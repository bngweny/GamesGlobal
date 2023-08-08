using GamesGlobal.Infrastructure.Interfaces;
using GamesGlobal.Infrastructure.Models;
using GamesGlobal.Models;
using GamesGlobal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GamesGlobal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListItemRepository _shoppingListItemRepository;
        private readonly IMinioService _minioService;
        private readonly GraphServiceClient _graphServiceClient;
        private readonly IUserRepository _userRepository;


        public ShoppingListController(IShoppingListItemRepository shoppingListItemRepository, IMinioService minioService, GraphServiceClient graphServiceClient, IUserRepository userRepository)
        {
            _shoppingListItemRepository = shoppingListItemRepository;
            _minioService = minioService;
            _graphServiceClient = graphServiceClient;
            _userRepository = userRepository;
        }

        // GET: api/<ShoppingListController>
        [HttpGet]
        public async Task<IEnumerable<ShoppingItem>> Get()
        {
            var user = await GetUser();
            return (await _shoppingListItemRepository.GetAllShoppingListItemsAsync())
                .Where(x => x.Username == user.Username);
        }

        // GET api/<ShoppingListController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) 
        {
            var user = await GetUser();
            var item = await _shoppingListItemRepository.GetShoppingListItemByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            if (user.Username != item.Username)
            {
                return Forbid();
            }

            return Ok(item);
        }

        // POST api/<ShoppingListController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ShoppingItemDto value)
        {
            var user = await GetUser();
            var item = await _shoppingListItemRepository.CreateShoppingListItemAsync(
                new ShoppingItem
                {
                    CreatedAt = DateTime.Now,
                    Description = value.Description,
                    ImageUrl = value.ImageUrl,
                    Name = value.Name,
                    UpdatedAt = DateTime.Now,
                    Username = user.Username,
                }
            );

            return Ok(item);
        }

        // PUT api/<ShoppingListController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ShoppingItemDto value)
        {
            var user = await GetUser();
            var existing = await _shoppingListItemRepository.GetShoppingListItemByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            if (user.Username != existing.Username)
            {
                return Forbid();
            }

            var item = await _shoppingListItemRepository.UpdateShoppingListItemAsync(
                id,
                new ShoppingItem
                {
                    CreatedAt = DateTime.Now,
                    Description = value.Description,
                    ImageUrl = value.ImageUrl,
                    Name = value.Name,
                    UpdatedAt = DateTime.Now,
                    Username = user.Username,
                }
            );
            return Ok(item);
        }

        // DELETE api/<ShoppingListController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await GetUser();
            var existing = await _shoppingListItemRepository.GetShoppingListItemByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            if (user.Username != existing.Username)
            {
                return Forbid();
            }

            await _shoppingListItemRepository.DeleteShoppingListItemAsync(id);

            return Ok();
        }

        [HttpPut("{itemId}/upload")]
        public async Task<IActionResult> UploadImage(int itemId, [FromForm] UploadImageDto model)
        {
            var user = await GetUser();
            var shoppingItem = await _shoppingListItemRepository.GetShoppingListItemByIdAsync(itemId);
            if (shoppingItem == null)
            {
                return NotFound();
            }

            if (user.Username != shoppingItem.Username)
            {
                return Forbid();
            }

            try
            {
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
                    var item = await _shoppingListItemRepository.UpdateShoppingListItemAsync(itemId, shoppingItem);

                    return Ok(item);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        private async Task<Infrastructure.Models.User> GetUser()
        {
            var user = await _graphServiceClient.Me.Request().GetAsync();
            var applicationUser = await _userRepository.GetUserByUsernameAsync(user.Id);

            if (applicationUser == null)
            {
                applicationUser = new Infrastructure.Models.User
                {
                    Email = user.Mail ?? user.UserPrincipalName,
                    Username = user.Id
                };

                await _userRepository.CreateUserAsync(applicationUser);
            }
            return applicationUser;
        }
    }
}
