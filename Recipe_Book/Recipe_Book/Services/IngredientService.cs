using Recipe_Book.Data;
using Recipe_Book.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Book.Services
{
    public class IngredientService
    {
        private readonly DBConnect _context;

        public IngredientService()
        {
            _context = new DBConnect();
        }

        public void AddIngredient(Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
            _context.SaveChanges();
        }
        public void AddIngredientToRecipeIngredients(Recipe recipe, int ingredientId, decimal quantity, int unitId)
        {
            recipe.RecipeIngredients.Add(new RecipeIngredient { IngredientId = ingredientId, Quantity = quantity, UnitId = unitId });
            _context.SaveChanges();
        }
        public void DeleteIngredient(int id)
        {
            var ingredient = _context.Ingredients.Find(id);
            if (ingredient != null)
            {
                _context.Ingredients.Remove(ingredient);
                _context.SaveChanges();
            }
        }
        public List<Ingredient> GetIngredientsByRecipe(Recipe recipe)
        {
            return _context.RecipeIngredients.Where(ri => ri.Recipe.Equals(recipe)).Select(ri => ri.Ingredient).ToList();
        }
        public List<Ingredient> GetAllIngredients()
        {
            return _context.Ingredients.ToList();
        }

    }
}
