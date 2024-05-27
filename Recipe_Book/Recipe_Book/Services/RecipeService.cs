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

        public List<Recipe> GetAllRecipes()
        {

            return _context.Recipes.Include(r => r.RecipeIngredients).ThenInclude(ri => ri.Ingredient).ToList();
        }

        public Recipe GetRecipeById(int id)
        {
            return _context.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefault(r => r.Id == id);
        }

        public  List<Recipe> GetRecipeByCategory(string category)
        {
            return _context.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .Where(r => r.Category == category)
                .ToList();
        }

        public List<Recipe> GetRecipeByIngredient(string ingredient)
        {
            return _context.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .Where(r => r.Ingredients.Contains(ingredient))
                .ToList();
        }

        public void AddRecipe(Recipe recipe, List<int> ingredientsIds, List<int> quantities, List<string> units)
        {
            recipe.AddDate = DateTime.Now;
            for (int i = 0; i < ingredientsIds.Count; i++)
            {
                recipe.RecipeIngredients.Add(new RecipeIngredient { IngredientId = ingredientsIds[i], Quantity = quantities[i], Unit = units[i] });
            }

            _context.Recipes.Add(recipe);
            _context.SaveChanges();
        }

        public void UpdateRecipe(Recipe recipe)
        {
            _context.Entry(recipe).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteRecipe(int id)
        {
            var recipe = _context.Recipes.Find(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
                _context.SaveChanges();
            }
        }

        public Ingredient GetIngredientById(int id)
        {
            return _context.Ingredients.Find(id);
        }
    }
}
