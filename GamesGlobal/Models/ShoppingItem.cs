namespace GamesGlobal.Models
{
    public class ShoppingItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool IsPurchased { get; set; }

        public string UserId { get; set; }
    }
}
