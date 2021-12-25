using System.Collections.Generic;
using System.Threading.Tasks;
using UserControl.Domain.Models;

namespace UserControl.Infra.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<List<User>> GetUsers();
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<User> DeleteUserAsync(int userId);
         Task<User> AuthenticateAsync(string username, string password);
    }
}