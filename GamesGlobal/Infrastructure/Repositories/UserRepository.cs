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

        public async Task UpdateUserAsync(User user)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            dbContext.Entry(user).State = EntityState.Modified;
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
