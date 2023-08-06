using Microsoft.AspNetCore.Identity;

namespace GamesGlobal.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<ShoppingItem>? Cart { get; set; }

    }
}
