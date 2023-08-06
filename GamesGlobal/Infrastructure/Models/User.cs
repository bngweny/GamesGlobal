using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamesGlobal.Infrastructure.Models
{
    [Table("Users")]
    public class User
    {
        [Required]
        [Key]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
