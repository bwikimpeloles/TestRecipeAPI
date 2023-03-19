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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasMany(t => t.TestRecipes)
                .WithMany(a => a.Accounts)
                .UsingEntity<FavouriteTable>(
                f => f.HasOne(prop => prop.TestRecipe).WithMany().HasForeignKey(prop => prop.TestRecipesId),
                f => f.HasOne(prop => prop.Account).WithMany().HasForeignKey(prop => prop.AccountsId),
                f =>
                {
                    f.HasKey(prop => new { prop.TestRecipesId, prop.AccountsId });
                });
                
                
                
                
                base.OnModelCreating(modelBuilder);
        }




        public DbSet<TestRecipe> TestRecipes => Set<TestRecipe>();
    
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<FavouriteTable> FavouriteTables => Set<FavouriteTable>();



    }
}
