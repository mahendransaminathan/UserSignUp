using Models;

namespace Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUserByEmail(string email);        
        Task<User> CreateUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(string email);
    }
}