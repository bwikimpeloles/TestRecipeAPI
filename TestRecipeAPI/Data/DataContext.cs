using Microsoft.EntityFrameworkCore;
using TestRecipeAPI.Entities;

namespace TestRecipeAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext() {}
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.EnableSensitiveDataLogging();

        public DbSet<User> Users { get; set; }

        private readonly IConfiguration Configuration;

        public DbSet<TestRecipe> TestRecipes => Set<TestRecipe>();
        public DbSet<Favourite> Favourites => Set<Favourite>();
        public DbSet<Account> Accounts => Set<Account>();




    }
}
