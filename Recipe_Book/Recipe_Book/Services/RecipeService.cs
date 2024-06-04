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

        public List<Recipe> GetAllRecipes()
        {

            return _context.Recipes.Include(r => r.RecipeIngredients).ThenInclude(ri => ri.Ingredient).Include(r => r.Category).ToList();
        }

        public void AddRecipe(Recipe recipe, List<int> ingredientsIds, List<decimal> quantities, List<int> unitsIds)
        {
            recipe.AddDate = DateTime.Now;
            for (int i = 0; i < ingredientsIds.Count; i++)
            {
                recipe.RecipeIngredients.Add(new RecipeIngredient { IngredientId = ingredientsIds[i], Quantity = quantities[i], UnitId = unitsIds[i] });
            }

            _context.Recipes.Add(recipe);
            _context.SaveChanges();
        }

        public void UpdateRecipe()
        {
            //Finish this
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
                .Where(r => r.CategoryId == categoryId)
                .ToList();
        }

        public List<Recipe> GetRecipesByIngredient(int ingredientId)
        {
            return _context.RecipeIngredients
             .Where(ri => ri.Ingredient.Id == ingredientId)
             .Select(ri => ri.Recipe)
             .Distinct()
             .ToList();
        }

        public List<Recipe> GetRecipesByMultipleIngredients(List<int> ingredientIds)
        {
            return _context.Recipes
                .Where(r => r.RecipeIngredients.Any(ri => ingredientIds.Contains(ri.Ingredient.Id)))
                .ToList();
        }

        public void AddIngredient(Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
            _context.SaveChanges();
        }
        public Ingredient GetIngredientById(int id)
        {
            return _context.Ingredients.Find(id);
        }
<<<<<<< Updated upstream
        public List<Ingredient> GetAllIngredients()
        {
            return _context.Ingredients.ToList();
        }

        public void AddCategory(Category category) 
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }
        public List<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }
        public Category GetCategoryByName(string categoryName)
        {
            return _context.Categories.FirstOrDefault(c => c.Name == categoryName);
        }

        public void AddUnit(Unit units)
        {
            _context.Units.Add(units);
            _context.SaveChanges();
        }
        public List<Unit> GetAllUnits()
        {
            return _context.Units.ToList();
        }
=======

        public Ingredient GetIngredientByName(string name)
        {
            return _context.Ingredients.FirstOrDefault(i => i.Name == name);
        }

        public void AddIngredient(Recipe recipe, Ingredient ingredient, int quantity, string unit)
        {
            _context.Ingredients.Add(ingredient);
            recipe.RecipeIngredients.Add(new RecipeIngredient { RecipeId = recipe.Id, IngredientId = ingredient.Id, Quantity = quantity, Unit = unit });
            _context.SaveChanges();
        }

        public void UpdateRecipeIngredientQuantity(Recipe recipe, Ingredient ingredient, int newQuantity)
        {
            _context.RecipeIngredients.FirstOrDefault(ri => ri.RecipeId == recipe.Id && ri.IngredientId == ingredient.Id).Quantity = newQuantity;
            _context.SaveChanges();
        }
>>>>>>> Stashed changes
    }
}
