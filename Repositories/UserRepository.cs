using Models;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>();

        public Task<IEnumerable<User>> GetUserByEmail(string email)
        {
            var users = _users.Where(u => u.Email == email);
            return Task.FromResult(users);
        }

        public Task<bool> UpdateUser(User user)
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

        public Task<User> CreateUser(User user)
        {
            _users.Add(user);
            return Task.FromResult(user);
        }

    }
}