using Recipe_Book.Services;
using Recipe_Book.Models;
using System.Text;
using CustomPrint;


namespace Recipe_Book.Views
{

    public class Display
    {
        private readonly RecipeService recipeService;
        private RecipeService @object;

        public Display()
        {
            this.recipeService = new RecipeService();
        }

        public Display(RecipeService @object)
        {
            this.@object = @object;
        }

        public void ShowMenu()
        {
            Console.OutputEncoding = Encoding.UTF8;
            bool exit = false;
            int choice;
            while (!exit)
            {
                MCP.PrintNL(" " + new string('*', 101), "green");
                MCP.PrintNL(" *" + new string(' ', 99) + "*", "green");
                MCP.PrintNL(" *" + new string(' ', 41)+"Каталог с рецепти".ToUpper()+ new string(' ', 41) + "*", "green");
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
                        MCP.PrintNL("Невалидна команда!","red");                     
                        break;

                }
            }
        }

        public void ShowAllRecipes()
        {
            MCP.PrintNL("Показване на списъка с всички рецепти: ", "red");
            var recipes = recipeService.GetAllRecipes();

            ShowRecipesInColor(recipes, "yellow");
        }

        public void SearchRecipeByCategory()
        {
            try
            {
                MCP.PrintNL("-----Категории-----", "yellow");
                var categories = recipeService.GetAllCategories();
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
                if (recipes == null)
                {
                    MCP.PrintNL("Няма рецепти в тази категория!", "yellow");
                    return;
                }
                ShowRecipesInColor(recipes, "yellow");
            }
            catch (Exception)
            {
                MCP.Print("Невалидно Id!", "red");
            }
        }

        public void SearchRecipeByIngredient()
        {
            try
            {
                MCP.PrintNL("-----Съставки-----", "yellow");
                var ingredients = recipeService.GetAllIngredients();
                foreach (var ingredient in ingredients)
                {
                    MCP.PrintNL($"{ingredient.Id}. {ingredient.Name}", "yellow");
                }
                MCP.Print("Въведете Id на съставката: ", "yellow");
                int choice = int.Parse(Console.ReadLine());
                if (!ingredients.Select(i => i.Id).Contains(choice))
                {
                    throw new Exception();
                }
                var recipes = recipeService.GetRecipesByIngredient(choice);
                if (recipes == null)
                {
                    MCP.PrintNL("Няма рецепти с тази съставка!", "yellow");
                    return;
                }
                ShowRecipesInColor(recipes, "yellow");

            }
            catch (Exception)
            {
                MCP.Print("Невалидно Id!", "red");
            }
        }

        private void SearchRecipesByMultipleIngredients()
        {
            try
            {
                MCP.PrintNL("-----Съставки-----", "yellow");
                var ingredients = recipeService.GetAllIngredients();
                foreach (var ingredient in ingredients)
                {
                    MCP.PrintNL($"{ingredient.Id}. {ingredient.Name}", "yellow");
                }
                List<int> ingredientIds = new List<int>();
                while (true)
                {
                    MCP.Print("Въведете Id на съставката(или оставете празно за край): ", "yellow");
                    string choice = Console.ReadLine();
                    if(string.IsNullOrEmpty(choice)) break;

                    int ingredientId = int.Parse(choice);
                    if (!ingredients.Select(i => i.Id).Contains(ingredientId))
                    {
                        throw new Exception();
                    }
                    ingredientIds.Add(ingredientId);
                }

                var recipes = recipeService.GetRecipesByMultipleIngredients(ingredientIds);
                if (recipes == null)
                {
                    MCP.PrintNL("Няма рецепти с тези съставки!", "yellow");
                    return;
                }
                ShowRecipesInColor(recipes, "yellow");

            }
            catch (Exception)
            {
                MCP.Print("Невалидно Id!", "red");
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
                    var categories = recipeService.GetAllCategories();
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
                    Console.WriteLine("3. Край на добавянето на съставки");
                    Console.Write("Изберете опция(1-3): ");
                    int choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine(new string('-', 50));
                            var currentIngredients = recipeService.GetAllIngredients();
                            foreach (var i in currentIngredients)
                            {
                                Console.WriteLine($"{i.Id}. {i.Name}");
                            }
                            Console.Write("Изберете Id на съставката: ");
                            int ingredientId = int.Parse(Console.ReadLine());
                            ingredientIds.Add(ingredientId);

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
                            unitIds.Add(AddUnit());
                            break;
                        case 2:
                            Console.WriteLine(new string('-', 50));
                            Console.Write("Въведете име на съставката: ");
                            string ingredientName = Console.ReadLine();
                            Ingredient ingredient = new Ingredient() { Name = ingredientName };
                            recipeService.AddIngredient(ingredient);
                            ingredientIds.Add(ingredient.Id);

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
                            unitIds.Add(AddUnit());
                            break;
                        case 3:
                            isAdding = false;
                            break;
                        default:
                            MCP.PrintNL("Невалидна команда", "red");
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

        private void UpdateRecipe()
        {
            try
            {
                MCP.PrintNL("Списък с всички рецепти: ", "yellow");
                ShowAllRecipes();

                MCP.Print("Въведи id на рецептата, което служи за редакция: ", "yellow");
                int id = int.Parse(Console.ReadLine());

                Recipe recipe = recipeService.GetRecipeById(id);
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
                var categories = recipeService.GetAllCategories();
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
                    recipe.Category = recipeService.GetCategoryById(choice);
                }

                //Finish for ingredients

                recipeService.UpdateRecipe();
                MCP.PrintNL("Рецептата е успешно редактирана!", "green");
            }
            catch (Exception)
            {
                MCP.PrintNL("Невалидни данни! Въведете цяло число за ID!", "red");
            }
        }

        private void DeleteRecipe()
        {

            try
            {
                MCP.PrintNL("Списък с всички рецепти: ", "red");
                ShowAllRecipes();

                MCP.Print("Въведи id, което служи за изтриване на рецепта: ", "red");
                int id = int.Parse(Console.ReadLine());

                recipeService.DeleteRecipe(id);
                MCP.PrintNL("Изтрита рецепта!", "red");
            }
            catch (Exception)
            {
                MCP.PrintNL("Невалидни данни! Въведете цяло число за ID!", "red");
            }
            
        }

        private void ShowRecipesInColor(List<Recipe> recipes, string color)
        {
            foreach (var recipe in recipes)
            {
                MCP.PrintNL("|" + new string('-', 150) + "|", color);
                MCP.PrintNL($"|ID: {recipe.Id} | Name: {recipe.Name} | Description: {recipe.Description} | Author: {recipe.Author} | Date: {recipe.AddDate} | Category: {recipe.Category.Name} | Ingredients: {string.Join(", ", recipe.RecipeIngredients.Select(ri => $"{ri.Ingredient.Name} {ri.Quantity} {ri.Unit.Name}"))} |", color);
            }
            MCP.PrintNL("|" + new string('-', 150) + "|", color);
        }

        private int AddUnit()
        {
            int unitId;

            while (true)
            {
                Console.WriteLine("-----Избор на мярка------");
                var units = recipeService.GetAllUnits();
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
