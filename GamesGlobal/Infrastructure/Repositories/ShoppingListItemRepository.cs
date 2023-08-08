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

        public async Task<ShoppingItem> CreateShoppingListItemAsync(ShoppingItem item)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            dbContext.ShoppingItems.Add(item);
            await dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<ShoppingItem> UpdateShoppingListItemAsync(int itemId, ShoppingItem shoppingItem)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var existingItem = await dbContext.ShoppingItems.FindAsync(itemId);

            if (existingItem == null)
            {
                throw new ArgumentException($"Shopping item with ItemId {itemId} not found.");
            }

            // Update the properties of the existing item with the new values
            existingItem.Name = shoppingItem.Name;
            existingItem.Description = shoppingItem.Description;
            existingItem.UpdatedAt = DateTime.UtcNow;

            dbContext.Entry(existingItem).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return existingItem;
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