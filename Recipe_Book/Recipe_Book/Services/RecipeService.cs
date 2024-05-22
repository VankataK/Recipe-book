using Microsoft.EntityFrameworkCore;
using Recipe_Book.Models;
using Recipe_Book.Data;

namespace Recipe_Book.Services
{
    public class RecipeService
    {
        private readonly DBConnect _context;

        public RecipeService(DBConnect context)
        {
            _context = context;
        }

        public async Task<List<Recipe>> GetAllRecipes()
        {
            return await _context.Recipes.Include(r => r.RecipeIngredients).ThenInclude(ri => ri.Ingredient).ToListAsync();
        }

        public async Task<Recipe> GetRecipeById(int id)
        {
            return await _context.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Recipe>> GetRecipeByCategory(string category)
        {
            return await _context.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .Where(r => r.Category == category)
                .ToListAsync();
        }

        public async Task<List<Recipe>> GetRecipeByIngredient(string ingredient)
        {
            return await _context.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .Where(r => r.Ingredients.Contains(ingredient))
                .ToListAsync();
        }

        public async Task AddRecipe(Recipe recipe, List<int> ingredientsIds, List<int> quantities, List<string> units)
        {
            recipe.AddDate = DateTime.Now;
            for (int i = 0; i < ingredientsIds.Count; i++)
            {
                recipe.RecipeIngredients.Add(new RecipeIngredient { IngredientId = ingredientsIds[i], Quantity = quantities[i], Unit = units[i] });
            }

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRecipe(Recipe recipe)
        {
            _context.Entry(recipe).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();
            }
        }
    }
}
