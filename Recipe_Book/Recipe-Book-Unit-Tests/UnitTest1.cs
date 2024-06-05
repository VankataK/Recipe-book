using NUnit.Framework;
using Recipe_Book.Models;
using Recipe_Book.Services;
using Recipe_Book.Views;
using System;
using System.Collections.Generic;
using System.IO;

namespace Recipe_Book.Tests
{
    public class FakeRecipeService : RecipeService
    {
        public List<Recipe> Recipes { get; set; } = new List<Recipe>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<Unit> Units { get; set; } = new List<Unit>();

        public void AddRecipe(Recipe recipe, List<int> ingredientIds, List<decimal> quantities, List<int> unitIds)
        {
            Recipes.Add(recipe);
        }

        public List<Category> GetAllCategories() => Categories;

        public List<Ingredient> GetAllIngredients() => Ingredients;

        public List<Unit> GetAllUnits() => Units;

        public List<Recipe> GetAllRecipes() => Recipes;

        public List<Recipe> GetRecipesByCategory(int categoryId) => Recipes;

        public List<Recipe> GetRecipesByIngredient(int ingredientId) => Recipes;
    }

    [TestFixture]
    public class DisplayTests
    {
        private FakeRecipeService _fakeRecipeService;
        private Display _display;

        [SetUp]
        public void Setup()
        {
            _fakeRecipeService = new FakeRecipeService();
            _display = new Display(_fakeRecipeService);
        }

        

        [Test]
        public void SearchRecipeByCategory_ValidCategory_ShouldCallGetRecipesByCategory()
        {
            // Arrange
            int categoryId = 1;
            _fakeRecipeService.Categories = new List<Category>
            {
                new Category { Id = categoryId, Name = "Desserts" }
            };
            _fakeRecipeService.Recipes = new List<Recipe>();

            // Mock Console input
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                using (var sr = new StringReader($"{categoryId}\n"))
                {
                    Console.SetIn(sr);

                    // Act
                    _display.SearchRecipeByCategory();
                }
            }
        }

        [Test]
        public void SearchRecipeByIngredient_ValidIngredient_ShouldCallGetRecipesByIngredient()
        {
            // Arrange
            int ingredientId = 1;
            _fakeRecipeService.Ingredients = new List<Ingredient>
            {
                new Ingredient { Id = ingredientId, Name = "Sugar" }
            };
            _fakeRecipeService.Recipes = new List<Recipe>();

            // Mock Console input
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                using (var sr = new StringReader($"{ingredientId}\n"))
                {
                    Console.SetIn(sr);

                    // Act
                    _display.SearchRecipeByIngredient();
                }
            }

            
        }

        [Test]
        public void AddNewRecipe_ShouldCallAddRecipe()
        {
            // Arrange
            var categories = new List<Category> { new Category { Id = 1, Name = "Desserts" } };
            var ingredients = new List<Ingredient> { new Ingredient { Id = 1, Name = "Sugar" } };
            var units = new List<Unit> { new Unit { Id = 1, Name = "Grams" } };

            _fakeRecipeService.Categories = categories;
            _fakeRecipeService.Ingredients = ingredients;
            _fakeRecipeService.Units = units;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                using (var sr = new StringReader("Chocolate Cake\nBake it well\nJohn Doe\n1\n1\n500\n1\n\n\n"))
                {
                    Console.SetIn(sr);

                    // Act
                    _display.AddNewRecipe();
                }
            }
        }
    }
}
