using Microsoft.EntityFrameworkCore;
using Recipe_Book.Models;
using Recipe_Book.Data;

namespace Recipe_Book.Services
{
    public class RecipeServices
    {
        private readonly DBConnect _context;

        public RecipeServices(DBConnect context)
        {
            _context = context;
        }

        public async Task<List<Recipe>> GetAllRecipes()
        {
            return await _context.Recipes.Include(r => r.RecipeIngredients).ThenInclude(ri => ri.Ingredient).ToListAsync();
        }

        public async Task<Recipe> GetAllRecipeById(int id)
        {
            return await _context.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Recipe> AddRecipe(Recipe recipe, List<int> ingredientsIds, List<decimal> amounts, List<string> units)
        {
            recipe.AddDate = DateTime.Now;
            for (int i = 0; i < ingredientsIds.Count; i++)
            {
                recipe.RecipeIngredients.Add(new RecipeIngredient { ingredientsId = ingredientsIds[i], Amo })
            }
        }
    }
}
