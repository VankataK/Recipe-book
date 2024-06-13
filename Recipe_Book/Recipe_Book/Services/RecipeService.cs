using Microsoft.EntityFrameworkCore;
using Recipe_Book.Models;
using Recipe_Book.Data;

namespace Recipe_Book.Services
{
    public class RecipeService
    {

        private readonly DBConnect _context;

        public RecipeService()
        {
            _context = new DBConnect();
        }
        public RecipeService(DBConnect db)
        {
            _context = db;
        }

        public List<Recipe> GetAllRecipes()
        {
            return _context.Recipes.Include(r => r.RecipeIngredients).ThenInclude(ri => ri.Unit).Include(r => r.RecipeIngredients).ThenInclude(ri => ri.Ingredient).Include(r => r.Category).ToList();
        }
        public void AddRecipe(Recipe recipe, List<int> ingredientsIds, List<decimal> quantities, List<int> unitsIds)
        {
            recipe.AddDate = DateTime.Now;
            for (int i = 0; i < ingredientsIds.Count; i++)
            {
                recipe.RecipeIngredients.Add(new RecipeIngredient { IngredientId = ingredientsIds[i], Quantity = quantities[i], UnitId = unitsIds[i]});
            }

            _context.Recipes.Add(recipe);
            _context.SaveChanges();
        }
        public void UpdateRecipe()
        {
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
        public Recipe GetRecipeById(int id)
        {
            return _context.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefault(r => r.Id == id);
        }
        public List<Recipe> GetRecipesByCategory(int categoryId)
        {
            return _context.Recipes
                .Include(r => r.Category)
                .Where(r => r.CategoryId == categoryId)
                .ToList();
        }
        public List<Recipe> GetRecipesByIngredient(Ingredient ingredient)
        {
            return _context.RecipeIngredients
             .Where(ri => ri.Ingredient == ingredient)
             .Select(ri => ri.Recipe)
             .Distinct()
             .ToList();
        }
        public List<Recipe> GetRecipesByMultipleIngredients(List<Ingredient> ingredients)
        {
            var recipes = GetAllRecipes();
            List<Recipe> result = new List<Recipe>();
            foreach (var recipe in recipes)
            {
                if (Check(recipe, ingredients))
                {
                    result.Add(recipe);
                }
            }
            return result;
        }


        private Func<Recipe, List<Ingredient>, bool> Check = (recipe, ing) =>
        {

            foreach (var i in ing)
            {
                bool isOK = false;
                foreach (var ii in recipe.RecipeIngredients)
                {
                    if (i.Name == ii.Ingredient.Name) { isOK = true; break; }
                }

                if (!isOK) return false;
            }

            return true;
        };

    }
}
