using GamesGlobal.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesGlobal.Infrastructure.DbContexts
{
    public class GamesGlobalDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<ShoppingItem> ShoppingItems { get; set; }

        public GamesGlobalDbContext(DbContextOptions<GamesGlobalDbContext> options) : base(options)
        {
        }
    }
}
