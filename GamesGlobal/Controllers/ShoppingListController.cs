using GamesGlobal.Infrastructure.DbContexts;
using GamesGlobal.Infrastructure.Interfaces;
using GamesGlobal.Infrastructure.Models;
using GamesGlobal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GamesGlobal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListItemRepository _shoppingListItemRepository;
        public ShoppingListController(IShoppingListItemRepository shoppingListItemRepository)
        {
            _shoppingListItemRepository = shoppingListItemRepository;
        }

        // GET: api/<ShoppingListController>
        [HttpGet]
        public async Task<IEnumerable<ShoppingItem>> Get()
        {
            return await _shoppingListItemRepository.GetAllShoppingListItemsAsync();
        }

        // GET api/<ShoppingListController>/5
        [HttpGet("{id}")]
        public async Task<ShoppingItem> Get(int itemId) 
        {
            return await _shoppingListItemRepository.GetShoppingListItemByIdAsync(itemId);
        }

        // POST api/<ShoppingListController>
        [HttpPost]
        public async void Post([FromBody] ShoppingItemDto value)
        {
            await _shoppingListItemRepository.CreateShoppingListItemAsync(
                new ShoppingItem
                {
                    CreatedAt = DateTime.Now,
                    Description = value.Description,
                    ImageUrl = value.ImageUrl,
                    Name = value.Name,
                    UpdatedAt = DateTime.Now,
                    Username = "user@user.com",
                }
            );
        }

        // PUT api/<ShoppingListController>/5
        [HttpPut("{id}")]
        public async void Put(int id, [FromBody] ShoppingItemDto value)
        {
            await _shoppingListItemRepository.UpdateShoppingListItemAsync(
                new ShoppingItem
                {
                    CreatedAt = DateTime.Now,
                    Description = value.Description,
                    ImageUrl = value.ImageUrl,
                    Name = value.Name,
                    UpdatedAt = DateTime.Now,
                    Username = "user@user.com",
                }
            );
        }

        // DELETE api/<ShoppingListController>/5
        [HttpDelete("{id}")]
        public async void Delete(int itemId)
        {
            await _shoppingListItemRepository.DeleteShoppingListItemAsync(itemId);
        }
    }
}
