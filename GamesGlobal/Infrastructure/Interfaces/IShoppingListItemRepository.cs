using GamesGlobal.Infrastructure.Models;

namespace GamesGlobal.Infrastructure.Interfaces
{
    public interface IShoppingListItemRepository
    {
        Task<ShoppingItem> GetShoppingListItemByIdAsync(int itemId);
        Task<IEnumerable<ShoppingItem>> GetAllShoppingListItemsAsync();
        Task<ShoppingItem> CreateShoppingListItemAsync(ShoppingItem item);
        Task<ShoppingItem> UpdateShoppingListItemAsync(int itemId, ShoppingItem item);
        Task DeleteShoppingListItemAsync(int itemId);
    }
}
