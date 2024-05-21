using Microsoft.EntityFrameworkCore;
using Recipe_Book.Models;

namespace Recipe_Book.Data
{
    public class DBConnect : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<Ingredient> Ingredients { get; set;}

        public DbSet<RecipeIngredients> RecipeIngredients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;database=recipebook;user=root;password=", 
                new MySqlServerVersion(new Version(10,4,27)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<RecipeIngredients>()
                .HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.RecipeId);

            modelBuilder
                .Entity<RecipeIngredients>()
                .HasOne(ri => ri.Ingredient)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.IngredientId);
        }


    }
}
