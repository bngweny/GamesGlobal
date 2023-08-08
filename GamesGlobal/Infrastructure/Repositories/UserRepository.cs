using GamesGlobal.Infrastructure.DbContexts;
using GamesGlobal.Infrastructure.Interfaces;
using GamesGlobal.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesGlobal.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbContextFactory<GamesGlobalDbContext> _dbContextFactory;

        public UserRepository(IDbContextFactory<GamesGlobalDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            return await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            return await dbContext.Users.ToListAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(string username, User user)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var existingUser = await dbContext.Users.FindAsync(username);

            if (existingUser == null)
            {
                throw new ArgumentException($"User with username {username} not found.");
            }

            // Update the properties of the existing user with the new values
            existingUser.Email = user.Email;

            dbContext.Entry(existingUser).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(string userId)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var user = await GetUserByUsernameAsync(userId);
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
        }
    }
}
