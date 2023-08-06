using GamesGlobal.Infrastructure.DbContexts;
using GamesGlobal.Infrastructure.Interfaces;
using GamesGlobal.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesGlobal.Infrastructure.Repositories
{
    public class ShoppingListItemRepository : IShoppingListItemRepository
    {
        private readonly IDbContextFactory<GamesGlobalDbContext> _dbContextFactory;

        public ShoppingListItemRepository(IDbContextFactory<GamesGlobalDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<ShoppingItem> GetShoppingListItemByIdAsync(int itemId)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            return await dbContext.ShoppingItems.FirstOrDefaultAsync(item => item.ItemId == itemId);
        }

        public async Task<IEnumerable<ShoppingItem>> GetAllShoppingListItemsAsync()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            return await dbContext.ShoppingItems.ToListAsync();
        }

        public async Task CreateShoppingListItemAsync(ShoppingItem item)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            dbContext.ShoppingItems.Add(item);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateShoppingListItemAsync(ShoppingItem item)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            dbContext.Entry(item).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteShoppingListItemAsync(int itemId)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var item = await GetShoppingListItemByIdAsync(itemId);

            dbContext.ShoppingItems.Remove(item);
            await dbContext.SaveChangesAsync();
        }
    }
}