using GamesGlobal.Infrastructure.Models;

namespace GamesGlobal.Infrastructure.Interfaces
{
    public interface IShoppingListItemRepository
    {
        Task<ShoppingItem> GetShoppingListItemByIdAsync(int itemId);
        Task<IEnumerable<ShoppingItem>> GetAllShoppingListItemsAsync();
        Task CreateShoppingListItemAsync(ShoppingItem item);
        Task UpdateShoppingListItemAsync(ShoppingItem item);
        Task DeleteShoppingListItemAsync(int itemId);
    }
}
