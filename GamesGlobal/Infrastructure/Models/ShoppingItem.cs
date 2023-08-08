using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamesGlobal.Infrastructure.Models
{
    [Table("ShoppingListItems")]
    public class ShoppingItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public string Username { get; set; }  // Foreign key to User table

    }
}
