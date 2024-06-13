using Recipe_Book.Services;
using Recipe_Book.Models;
using System.Text;
using CustomPrint;

namespace Recipe_Book.Views
{
    public class Display
    {

        private readonly RecipeService recipeService;
        private readonly IngredientService ingredientService;
        private readonly CategoryService categoryService;
        private readonly UnitService unitService;

        public Display()
        {
            this.recipeService = new RecipeService();
            this.ingredientService = new IngredientService();
            this.categoryService = new CategoryService();
            this.unitService = new UnitService();
            categoryService.AddCategoriesToDB();
            unitService.AddUnitsToDB();
        }

        public void ShowMenu()
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
                bool exit = false;
                int choice;
                while (!exit)
                {
                    MCP.PrintNL(" " + new string('*', 101), "green");
                    MCP.PrintNL(" *" + new string(' ', 99) + "*", "green");
                    MCP.PrintNL(" *" + new string(' ', 41) + "Каталог с рецепти".ToUpper() + new string(' ', 41) + "*", "green");
                    MCP.PrintNL(" *" + new string(' ', 99) + "*", "green");
                    MCP.PrintNL(" " + new string('*', 101), "green");


                    MCP.PrintNL(" 1. ПОКАЖИ ВСИЧКИ РЕЦЕПТИ", "cyan");
                    MCP.PrintNL(" 2. ТЪРСИ РЕЦЕПТИ ПО КАТЕГОРИЯ", "cyan");
                    MCP.PrintNL(" 3. ТЪРСИ РЕЦЕПТИ ПО ПРОДУКТ", "cyan");
                    MCP.PrintNL(" 4. ТЪРСИ РЕЦЕПТИ ПО МНОЖЕСТВО ПРОДУКТИ", "cyan");
                    MCP.PrintNL(" 5. ДОБАВИ НОВА РЕЦЕПТА", "cyan");
                    MCP.PrintNL(" 6. РЕДАКТИРАЙ РЕЦЕПТА", "cyan");
                    MCP.PrintNL(" 7. ИЗТРИЙ РЕЦЕПТА", "cyan");
                    MCP.PrintNL(" 8. ИЗХОД", "cyan");

                    MCP.Print("Въведете номер на команда (1-8): ", "cyan");

                    choice = int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            ShowAllRecipes();
                            break;

                        case 2:
                            SearchRecipeByCategory();
                            break;

                        case 3:
                            SearchRecipeByIngredient();
                            break;

                        case 4:
                            SearchRecipesByMultipleIngredients();
                            break;

                        case 5:
                            AddNewRecipe();
                            break;

                        case 6:
                            UpdateRecipe();
                            break;

                        case 7:
                            DeleteRecipe();
                            break;
                        case 8:
                            exit = true;
                            break;
                        default:
                            MCP.PrintNL("Невалидна команда!", "red");
                            break;

                    }
                }
            }
            catch (Exception)
            {
                MCP.PrintNL("Невалидна команда!(Нямаш мярка!!!)", "red");
                ShowMenu();
            }
        }

        public void ShowAllRecipes()
        {
            MCP.PrintNL("Показване на списъка с всички рецепти: ", "green");
            var recipes = recipeService.GetAllRecipes();
            if (recipes.Count == 0)
            {
                MCP.PrintNL("Няма рецепти", "red");
                return;
            }

            ShowRecipesInColor(recipes, "yellow");
        }

        public void SearchRecipeByCategory()
        {
            try
            {
                MCP.PrintNL("-----Категории-----", "yellow");
                var categories = categoryService.GetAllCategories();
                foreach (var category in categories)
                {
                    MCP.PrintNL($"{category.Id}. {category.Name}", "yellow");
                }
                MCP.Print("Въведете Id на категорията: ", "yellow");
                int choice = int.Parse(Console.ReadLine());
                if (!categories.Select(c => c.Id).Contains(choice))
                {
                    throw new Exception();
                }
                var recipes = recipeService.GetRecipesByCategory(choice);
                if (recipes.Count == 0)
                {
                    MCP.PrintNL("Няма рецепти в тази категория!", "yellow");
                    return;
                }
                ShowRecipesInColor(recipes, "yellow");
            }
            catch (Exception)
            {
                MCP.PrintNL("Невалидно Id!", "red");
            }
        }

        public void SearchRecipeByIngredient()
        {
            try
            {
                MCP.PrintNL("-----Съставки-----", "yellow");
                var ingredients = ingredientService.GetAllIngredients();
                foreach (var ingredient in ingredients)
                {
                    MCP.PrintNL($"{ingredients.IndexOf(ingredient)+1}. {ingredient.Name}", "yellow");
                }
                MCP.Print("Въведете номер на съставката: ", "yellow");
                int choice = int.Parse(Console.ReadLine())-1;
                if (ingredients.ElementAtOrDefault(choice) == default)
                {
                    throw new Exception();
                }
                var recipes = recipeService.GetRecipesByIngredient(ingredients[choice]);
                if (recipes.Count == 0)
                {
                    MCP.PrintNL("Няма рецепти с тази съставка!", "yellow");
                    return;
                }
                ShowRecipesInColor(recipes, "yellow");

            }
            catch (Exception)
            {
                MCP.PrintNL("Невалидно Id!", "red");
            }
        }

        public void SearchRecipesByMultipleIngredients()
        {
            try
            {
                MCP.PrintNL("-----Съставки-----", "yellow");
                var ingredients = ingredientService.GetAllIngredients();
                foreach (var ingredient in ingredients)
                {
                    MCP.PrintNL($"{ingredients.IndexOf(ingredient)+1}. {ingredient.Name}", "yellow");
                }
                List<Ingredient> chosenIngredients = new List<Ingredient>();
                while (true)
                {
                    MCP.Print("Въведете номер на съставката(или оставете празно за край): ", "yellow");
                    string choice = Console.ReadLine();
                    if(string.IsNullOrEmpty(choice)) break;

                    int ingredientNum = int.Parse(choice)+1;
                    if (ingredients.ElementAtOrDefault(ingredientNum) == default)
                    {
                        throw new Exception();
                    }
                    chosenIngredients.Add(ingredients[ingredientNum]);
                }

                var recipes = recipeService.GetRecipesByMultipleIngredients(chosenIngredients);
                if (recipes.Count == 0)
                {
                    MCP.PrintNL("Няма рецепти с тези съставки!", "yellow");
                    return;
                }
                ShowRecipesInColor(recipes, "yellow");

            }
            catch (Exception)
            {
                MCP.PrintNL("Невалидно Id!", "red");
            }
        }

        public void AddNewRecipe()
        {
            try
            {
                string name, description, author, categoryName;
                List<int> ingredientIds = new List<int>();
                List<decimal> quantities = new List<decimal>();
                List<int> unitIds = new List<int>();

                Console.Write("Име на рецептата: ");
                name = Console.ReadLine();

                Console.Write("Начин на приготвяне: ");
                description = Console.ReadLine();

                Console.Write("Автор: ");
                author = Console.ReadLine();
                int categoryId;
                while (true)
                {
                    MCP.PrintNL("-----Категории-----", "yellow");
                    var categories = categoryService.GetAllCategories();
                    foreach (var category in categories)
                    {
                        MCP.PrintNL($"{category.Id}. {category.Name}", "yellow");
                    }
                    MCP.Print("Въведете Id на категорията: ", "yellow");
                    categoryId = int.Parse(Console.ReadLine());
                    if (categories.Select(c => c.Id).Contains(categoryId))
                    {
                        break;
                    }
                    MCP.PrintNL("Невалидно Id!", "red");
                }
                bool isAdding = true;
                while (isAdding)
                {
                    Console.WriteLine("-----Избор на съставки-----");
                    Console.WriteLine("1. Избор от наличните съставки");
                    Console.WriteLine("2. Добавяне на нова съставка");
                    Console.Write("Изберете опция (или празно за край): ");
                    if (!int.TryParse(Console.ReadLine(),out int choice))
                    {
                        break;
                    }
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine(new string('-', 50));
                            var currentIngredients = ingredientService.GetAllIngredients();
                            foreach (var i in currentIngredients)
                            {
                                Console.WriteLine($"{currentIngredients.IndexOf(i)+1}. {i.Name}");
                            }
                            
                            Console.Write("Изберете номер на съставката (или остави празно за стъпка назад): ");
                            if (!int.TryParse(Console.ReadLine(),out int ingredientId))
                            {
                                break;
                            }

                            ingredientIds.Add(currentIngredients[ingredientId-1].Id);
                            
                            unitIds.Add(ChooseUnit());

                            Console.Write("Въведете количество: ");
                            if (int.TryParse(Console.ReadLine(), out int quantity))
                            {
                                quantities.Add(quantity);
                            }
                            else
                            {
                                MCP.PrintNL("Невалидно количество!", "red");
                                continue;
                            }
                            break;
                        case 2:
                            Console.WriteLine(new string('-', 50));
                            Console.Write("Въведете име на съставката: ");
                            string ingredientName = Console.ReadLine();
                            Ingredient ingredient = new Ingredient() { Name = ingredientName };
                            ingredientService.AddIngredient(ingredient);
                            ingredientIds.Add(ingredient.Id);

                            unitIds.Add(ChooseUnit());
                            Console.Write("Количество: ");
                            if (int.TryParse(Console.ReadLine(), out quantity))
                            {
                                quantities.Add(quantity);
                            }
                            else
                            {
                                MCP.PrintNL("Невалидно количество!", "red");
                                continue;
                            }
                            break;
                        default:
                            isAdding = false;
                            break;
                    }
                    
                }

                Recipe recipe = new Recipe(name, description, author, categoryId);
                recipeService.AddRecipe(recipe, ingredientIds, quantities, unitIds);
                MCP.PrintNL("Успешно добавена нова рецепта!", "green");
            }
            catch (Exception ex)
            {
                MCP.PrintNL($"Възникна грешка: {ex.Message}", "red");
            }
        } 

        public void UpdateRecipe()
        {
            try
            {
                MCP.PrintNL("Списък с всички рецепти: ", "yellow");
                ShowAllRecipes();

                MCP.Print("Въведи id на рецептата, което служи за редакция: ", "yellow");
                int id = int.Parse(Console.ReadLine());

                Recipe recipe = recipeService.GetRecipeById(recipeService.GetAllRecipes()[id-1].Id);
                if (recipe == null)
                {
                    MCP.PrintNL("Рецептата с това ID не е намерена!", "yellow");
                    return;
                }

                MCP.PrintNL($"Редактиране на рецепта: {recipe.Name}", "yellow");

                MCP.Print("Име на рецептата: (оставете празно за без промяна) ", "yellow");
                string name = Console.ReadLine();
                if (!string.IsNullOrEmpty(name))
                {
                    recipe.Name = name;
                }

                MCP.Print("Начин на приготвяне: (оставете празно за без промяна) ", "yellow");
                string description = Console.ReadLine();
                if (!string.IsNullOrEmpty(description))
                {
                    recipe.Description = description;
                }

                MCP.Print("Автор: (оставете празно за без промяна) ", "yellow");
                string author = Console.ReadLine();
                if (!string.IsNullOrEmpty(author))
                {
                    recipe.Author = author;
                }

                MCP.PrintNL("-----Категории-----", "yellow");
                var categories = categoryService.GetAllCategories();
                foreach (var category in categories)
                {
                    MCP.PrintNL($"{category.Id}. {category.Name}", "yellow");
                }
                MCP.Print("Въведете Id на категорията(или празно за без промяна): ", "yellow");
                string input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    int choice  = int.Parse(input);
                    recipe.CategoryId = choice;
                    recipe.Category = categoryService.GetCategoryById(choice);
                }

                bool isUpdating = true;

                while (isUpdating)
                {
                    Console.WriteLine("-----Редактиране на съставки-----");
                    Console.WriteLine("1. Премахване на съставка");
                    Console.WriteLine("2. Добавяне на нова съставка");
                    Console.Write("Изберете опция(или остави празно за край): ");

                    if (!int.TryParse(Console.ReadLine(), out int choice))
                    {
                        break;
                    }

                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine(new string('-', 50));
                            var recipreIngredients = ingredientService.GetIngredientsByRecipe(recipe);
                            foreach (var i in recipreIngredients)
                            {
                                Console.WriteLine($"{recipreIngredients.IndexOf(i) + 1}. {i.Name}");
                            }
                            Console.Write("Изберете номер на съставката: ");
                            int ingredientId = int.Parse(Console.ReadLine());
                            if (recipreIngredients.ElementAtOrDefault(ingredientId) == default)
                            {
                                MCP.PrintNL("Невалидно Id!", "red");
                            }
                            ingredientService.DeleteIngredient(recipreIngredients[ingredientId-1].Id);
                            break;

                        case 2:
                            Console.WriteLine(new string('-', 50));
                            Console.Write("Въведете име на съставката: ");
                            string ingredientName = Console.ReadLine();
                            Ingredient ingredient = new Ingredient() { Name = ingredientName };
                            ingredientService.AddIngredient(ingredient);
                            int unitId = ChooseUnit();
                            Console.Write("Количество: ");
                            if (!int.TryParse(Console.ReadLine(), out int quantity))
                            {
                                MCP.PrintNL("Невалидно количество!", "red");
                                continue;
                            }
                            ingredientService.AddIngredientToRecipeIngredients(recipe, ingredient.Id, quantity, unitId);
                            break;
                        default:
                            isUpdating = false;
                            break;
                    }
                    
                }

                recipeService.UpdateRecipe();
                MCP.PrintNL("Рецептата е успешно редактирана!", "green");
            }
            catch (Exception)
            {
                MCP.PrintNL("Невалидни данни! Въведете цяло число за ID!", "red");
            }
        }

        public void DeleteRecipe()
        {

            try
            {
                MCP.PrintNL("Списък с всички рецепти: ", "red");
                ShowAllRecipes();

                MCP.Print("Въведи id, което служи за изтриване на рецепта: ", "red");
                int id = int.Parse(Console.ReadLine());

                recipeService.DeleteRecipe(recipeService.GetAllRecipes()[id - 1].Id);
                MCP.PrintNL("Успешно изтрита рецепта!", "green");
            }
            catch (Exception)
            {
                MCP.PrintNL("Невалидни данни! Въведете цяло число за ID!", "red");
            }
            
        }

        public void ShowRecipesInColor(List<Recipe> recipes, string color)
        {
            int maxLength = recipes.Select(r => r.Description.Length).Max() + 30;
            MCP print = new MCP(color, maxLength, "left", "|", "|");
            foreach (var recipe in recipes)
            {
                print.PrintNL(new string('-', maxLength));
                print.PrintNL($"ID: {recipes.IndexOf(recipe)+1}");
                print.PrintNL($"Име: {recipe.Name}");
                print.PrintNL($"Начин на приготвяне: {recipe.Description}");
                print.PrintNL($"Автор: {recipe.Author}");
                print.PrintNL($"Дата на добавяне: {recipe.AddDate}");
                print.PrintNL($"Категория: {recipe.Category.Name}");
                print.PrintNL($"Съставки: {string.Join(", ", recipe.RecipeIngredients.Select(ri => $"{ri.Ingredient.Name} {ri.Quantity:0.00} {ri.Unit.Name}"))}");

            }
            print.PrintNL(new string('-', maxLength));
        }

        public int ChooseUnit()
        {
            int unitId;

            while (true)
            {
                Console.WriteLine("-----Избор на мярка------");
                var units = unitService.GetAllUnits();
                foreach (var unit in units)
                {
                    Console.WriteLine($"{unit.Id} {unit.Name}");
                }
                Console.Write("Изберете Id на мярката: ");
                unitId = int.Parse(Console.ReadLine());
                if (units.FirstOrDefault(u=> u.Id == unitId) != null)
                {
                    break;
                }
                MCP.PrintNL("Невалидно Id!", "red");
            }
            return unitId;
        }
    }
}
