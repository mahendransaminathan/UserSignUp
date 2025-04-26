using System.ComponentModel.DataAnnotations.Schema;
using Models;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<Models.User> _users = new List<Models.User>();

        private readonly CosmosClient _cosmosClient;

        private readonly Database _database;
        private readonly Container _container;

        public UserRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            _cosmosClient = cosmosClient;
            var databaseName = Environment.GetEnvironmentVariable("CosmosDB:DatabaseName") 
                               ?? configuration["CosmosDB:DatabaseName"];

            var containerName = Environment.GetEnvironmentVariable("CosmosDB:ContainerName") 
                                ?? configuration["CosmosDB:ContainerName"];

            _database = _cosmosClient.GetDatabase(databaseName);
            _container = _database.GetContainer(containerName);
        }
        
        public Task<IEnumerable<Models.User>> GetUserByEmail(string email)
        {
            return Task.FromResult(_container.GetItemLinqQueryable<Models.User>().Where(u => u.Email == email).ToList().AsEnumerable());
        }

        public Task<bool> UpdateUser(Models.User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser != null)
            {
                existingUser.Password = user.Password;
                existingUser.UserType = user.UserType;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }    

        public Task<bool> DeleteUser(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                _users.Remove(user);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<Models.User> CreateUser(Models.User user)
        {
            // _users.Add(user);
            return Task.FromResult(_container.CreateItemAsync(user, new PartitionKey(user.Email)).Result.Resource);
        }

    }
}