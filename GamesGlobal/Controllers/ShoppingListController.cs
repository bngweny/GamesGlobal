using GamesGlobal.Infrastructure.DbContexts;
using GamesGlobal.Infrastructure.Interfaces;
using GamesGlobal.Infrastructure.Models;
using GamesGlobal.Models;
using GamesGlobal.Services;
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
        private readonly IMinioService _minioService;

        public ShoppingListController(IShoppingListItemRepository shoppingListItemRepository, IMinioService minioService)
        {
            _shoppingListItemRepository = shoppingListItemRepository;
            _minioService = minioService;
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

        [HttpPost("{itemId}/upload")]
        public async Task<IActionResult> UploadImage(int itemId, [FromForm] UploadImageDto model)
        {
            try
            {
                var shoppingItem = await _shoppingListItemRepository.GetShoppingListItemByIdAsync(itemId);
                if (shoppingItem == null)
                {
                    return NotFound();
                }

                if (model.Image == null || model.Image.Length <= 0)
                {
                    return BadRequest("Image file is missing or empty.");
                }

                using (var memoryStream = new MemoryStream())
                {
                    await model.Image.CopyToAsync(memoryStream);
                    var objectName = $"shopping_item_{itemId}_{Guid.NewGuid()}.jpg"; 
                    var bucketName = "shopping-item-images";
                    var contentType = "image/jpg";

                    await _minioService.UploadObjectAsync(bucketName, objectName, contentType);

                    shoppingItem.ImageUrl = objectName;
                    await _shoppingListItemRepository.UpdateShoppingListItemAsync(shoppingItem);

                    return Ok(new { Message = "Image uploaded successfully." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
