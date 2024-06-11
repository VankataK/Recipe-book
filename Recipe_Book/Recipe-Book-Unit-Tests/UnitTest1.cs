using Microsoft.EntityFrameworkCore;
using Recipe_Book.Data;
using Recipe_Book.Models;
using Recipe_Book.Services;

namespace Recipe_Book.Tests
{
    [TestFixture]
    public class RecipeServiceTests
    {
        private RecipeService recipeService;
        private DBConnect _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DBConnect>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new DBConnect(options);
            _context.Database.EnsureCreated();

            recipeService = new RecipeService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void GetAllRecipes_ShouldReturnAllRecipes()
        {
            _context.Recipes.Add(new Recipe { Name = "Рецепта1", Description = "Начин на приготвяне 1" });
            _context.Recipes.Add(new Recipe { Name = "Рецепта2", Description = "Начин на приготвяне 2" });
            _context.SaveChanges();

            var result = recipeService.GetAllRecipes();

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetRecipeById_ShouldReturnCorrectRecipe()
        {
            var recipe = new Recipe { Id = 1, Name = "Рецепта", Description = "Начин на приготвяне" };
            _context.Recipes.Add(recipe);
            _context.SaveChanges();

            var result = recipeService.GetRecipeById(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Рецепта"));
        }

        [Test]
        public void GetRecipeById_InvalidId_ShouldReturnNull()
        {
            var result = recipeService.GetRecipeById(99);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void UpdateRecipe_ShouldUpdateRecipeDetails()
        {
            var recipe = new Recipe { Id = 1, Name = "Стара репецта", Description = "Стар начин на приготвяне" };
            _context.Recipes.Add(recipe);
            _context.SaveChanges();

            recipe.Name = "Нова рецепта";
            recipe.Description = "Нов начин на приготвяне";
            recipeService.UpdateRecipe();

            var updatedRecipe = _context.Recipes.FirstOrDefault(r => r.Id == 1);
            Assert.That(updatedRecipe, Is.Not.Null);
            Assert.That(updatedRecipe.Name, Is.EqualTo("Нова рецепта"));
            Assert.That(updatedRecipe.Description, Is.EqualTo("Нов начин на приготвяне"));
        }

        [Test]
        public void DeleteRecipe_ShouldRemoveRecipe()
        {
            var recipe = new Recipe { Id = 1, Name = "Рецепта за изтриване", Description = "Начин на приготвяне" };
            _context.Recipes.Add(recipe);
            _context.SaveChanges();

            recipeService.DeleteRecipe(1);

            var result = _context.Recipes.FirstOrDefault(r => r.Id == 1);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void AddRecipeWithIngredients_ShouldAddRecipeAndIngredients()
        {
            List<int> ingredientIds = new List<int>() { 1, 2 };
            List<decimal> quantities = new List<decimal>() { 10, 20 };
            List<int> unitIds = new List<int>() { 1, 3 };
            var recipe = new Recipe
            {
                Name = "Рецепта",
                Description = "Начин на приготвяне",
                Author = "Автор",
                CategoryId = 1
            };

            recipeService.AddRecipe(recipe, ingredientIds, quantities, unitIds);

            var result = _context.Recipes.FirstOrDefault(r => r.Name == "Рецепта");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RecipeIngredients.Count, Is.EqualTo(2));
        }
    }

    [TestFixture]
    public class CategoryServiceTests
    {
        private DBConnect _context;
        private CategoryService _categoryService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DBConnect>()
                .UseInMemoryDatabase(databaseName: "ТердтоваБаза")
                .Options;

            _context = new DBConnect(options);
            _context.Database.EnsureCreated();

            _categoryService = new CategoryService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void AddCategoriesToDB_ShouldAddAllCategories()
        {
            _categoryService.AddCategoriesToDB();

            var categories = _context.Categories.ToList();

            Assert.That(categories.Count, Is.EqualTo(7));
            Assert.That(categories.Any(c => c.Name == "Предястие"), Is.True);
        }

        [Test]
        public void GetCategoryById_ShouldReturnCorrectCategory()
        {
            var category = new Category { Name = "TestCategory" };
            _context.Categories.Add(category);
            _context.SaveChanges();

            var result = _categoryService.GetCategoryById(category.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("TestCategory"));
        }

        [Test]
        public void GetAllCategories_ShouldReturnAllCategories()
        {
            _context.Categories.Add(new Category { Name = "Category1" });
            _context.Categories.Add(new Category { Name = "Category2" });
            _context.SaveChanges();

            var result = _categoryService.GetAllCategories();

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetCategoryById_InvalidId_ShouldReturnNull()
        {
            var result = _categoryService.GetCategoryById(99);

            Assert.That(result, Is.Not.Null);
        }
    }
}
