﻿using Microsoft.EntityFrameworkCore;
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
    }
}
