using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserControl.Domain.Models;
using UserControl.Infra.Context;

namespace UserControl.Infra.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly UserDbContext _dbContext;

        public UserRepository(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _dbContext.Users
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(t => t.Id == userId);
        }

        public async Task<List<User>> GetUsers()
        {
            return await _dbContext.Users
                                   .AsNoTracking()
                                   .ToListAsync();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _dbContext.Entry<User>(user).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> DeleteUserAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            _dbContext.Remove(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = _dbContext.Users.Where(x => x.Username == username &&
												x.Password == x.Password)
												.FirstOrDefault();
            return user;
        }
    }
}