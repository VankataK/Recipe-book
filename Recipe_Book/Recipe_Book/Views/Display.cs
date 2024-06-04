using Recipe_Book.Services;
using Recipe_Book.Models;
using System.Text;
using CustomPrint;


namespace Recipe_Book.Views
{

    public class Display
    {

        private readonly RecipeService recipeService;

        public Display()
        {
            this.recipeService = new RecipeService();
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
                        SearchRecipesByMultipleProducts();
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

        private void SearchRecipeByCategory()
        {
            try
            {
                MCP.PrintNL("-----Категории-----", "magenta");
                var categories = recipeService.GetAllCategories();
                foreach (var category in categories)
                {
                    MCP.PrintNL($"{category.Id}. {category.Name}", "magenta");
                }
                MCP.Print("Въведете Id на категорията: ", "magenta");
                int choice = int.Parse(Console.ReadLine());
                if (!categories.Select(c => c.Id).Contains(choice))
                {
                    throw new Exception();
                }
                var recipes = recipeService.GetRecipesByCategory(choice);
                if (recipes == null)
                {
                    MCP.PrintNL("Няма рецепти в тази категория!", "magenta");
                    return;
                }
                ShowRecipesInColor(recipes, "magenta");
            }
            catch (Exception)
            {
                MCP.Print("Невалидно Id!", "red");
            }
        }

        private void SearchRecipeByProduct()
        {
            try
            {
                MCP.PrintNL("-----Съставки-----", "magenta");
                var ingredients = recipeService.GetAllIngredients();
                foreach (var ingredient in ingredients)
                {
                    MCP.PrintNL($"{ingredient.Id}. {ingredient.Name}", "magenta");
                }
                MCP.Print("Въведете Id на съставката: ", "magenta");
                int choice = int.Parse(Console.ReadLine());
                if (!ingredients.Select(i => i.Id).Contains(choice))
                {
                    throw new Exception();
                }
                var recipes = recipeService.GetRecipesByIngredient(choice);
                if (recipes == null)
                {
                    MCP.PrintNL("Няма рецепти с тази съставка!", "magenta");
                    return;
                }
                ShowRecipesInColor(recipes, "magenta");

            }
            catch (Exception)
            {
                MCP.Print("Невалидно Id!", "red");
            }
        }

        private void SearchRecipesByMultipleProducts()
        {
            try
            {
                MCP.PrintNL("-----Съставки-----", "magenta");
                var ingredients = recipeService.GetAllIngredients();
                foreach (var ingredient in ingredients)
                {
                    MCP.PrintNL($"{ingredient.Id}. {ingredient.Name}", "magenta");
                }
                List<int> ingredientIds = new List<int>();
                while (true)
                {
                    MCP.Print("Въведете Id на съставката(или оставете празно за край): ", "magenta");
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
                    MCP.PrintNL("Няма рецепти с тези съставки!", "magenta");
                    return;
                }
                ShowRecipesInColor(recipes, "magenta");

            }
            catch (Exception)
            {
                MCP.Print("Невалидно Id!", "red");
            }
        }

        private void AddNewRecipe()
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

                Console.Write("Категория: ");
                categoryName = Console.ReadLine().ToLower();
                var categories = recipeService.GetAllCategories();
                Category category = new Category { Name = categoryName };
                if (!categories.Select(c => c.Name).Contains(categoryName))
                {
                    recipeService.AddCategory(category);
                }

                while (true)
                {
                    Console.WriteLine("-----Избор на съставки-----");
                    Console.WriteLine("1. Избор от наличните съставки");
                    Console.WriteLine("2. Добавяне на нова съставка");
                    Console.WriteLine("3. Край на добавянето на съставки");
                    Console.Write("Изберете опция(1-3): ");
                    int choice = int.Parse(Console.ReadLine());
                    
                    if (choice == 3) break;
                    if (choice == 1)
                    {
                        Console.WriteLine(new string('-',50));
                        var currentIngredients = recipeService.GetAllIngredients();
                        foreach (var ingredient in currentIngredients)
                        {
                            Console.WriteLine($"{ingredient.Id}. {ingredient.Name}");
                        }
                        Console.Write("Изберете Id на съставката: ");
                        int ingredientId = int.Parse(Console.ReadLine());
                        ingredientIds.Add(ingredientId);

                        Console.Write("Количество: ");
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
                    }
                    if (choice == 2) 
                    {
                        Console.WriteLine(new string('-', 50));
                        Console.Write("Въведете име на съставката: ");
                        string ingredientName = Console.ReadLine();
                        Ingredient ingredient = new Ingredient() { Name = ingredientName };
                        recipeService.AddIngredient(ingredient);
                        ingredientIds.Add(ingredient.Id);

                        Console.Write("Количество: ");
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
                    }
                    
                    
                }

                Recipe recipe = new Recipe(name, description, author, category.Id);
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
                MCP.PrintNL("Списък с всички рецепти: ", "red");
                ShowAllRecipes();

                MCP.Print("Въведи id на рецептата, което служи за редакция: ", "red");
                int id = int.Parse(Console.ReadLine());

                Recipe recipe = recipeService.GetRecipeById(id);
                if (recipe == null)
                {
                    MCP.PrintNL("Рецептата с това ID не е намерена!", "red");
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

                MCP.Print("Категория: (оставете празно за без промяна) ", "yellow");
                string categoryName = Console.ReadLine();
                if (!string.IsNullOrEmpty(categoryName))
                {
                    categoryName = Console.ReadLine().ToLower();
                    if (recipeService.GetCategoryByName(categoryName) == null)
                    {
                        Category category = new Category { Name = categoryName };
                        recipeService.AddCategory(category);
                        recipe.Category = category;
                    }
                    else
                    {
                        recipe.Category = recipeService.GetCategoryByName(categoryName);
                    }
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
            {
                string category = "";
                MCP.PrintNL("Списък с всички рецепти: ", "red");
                ShowAllRecipes();

                MCP.Print("Въведи id на рецептата, което служи за редакция: ", "red");
                int id = int.Parse(Console.ReadLine());

                Recipe recipe = recipeService.GetRecipeById(id);
                if (recipe == null)
                {
                    MCP.PrintNL("Рецептата с това ID не е намерена!", "red");
                    return;
                }

                MCP.PrintNL($"Редактиране на рецепта: {recipe.Name}", "yellow");

                MCP.Print("Име на рецептата: (оставете празно за без промяна) ", "yellow");
                string food = Console.ReadLine();
                if (!string.IsNullOrEmpty(food))
                {
                    recipe.Name = food;
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

                MCP.PrintNL("Избери една от следните категории: ", "cyan");
                MCP.PrintNL("1. основно ястие", "green");
                MCP.PrintNL("2. салата", "green");
                MCP.PrintNL("3. супа", "green");
                MCP.PrintNL("4. десерт", "green");
                MCP.PrintNL("5. предястие", "green");
                MCP.PrintNL("6. рибно ястие", "green");
                MCP.PrintNL("7. апетайзер", "green");

                int choiceCategory = int.Parse(Console.ReadLine());
                switch (choiceCategory)
                {
                    case 1:
                        category = "основно ястие";
                        break;
                    case 2:
                        category = "салата";
                        break;
                    case 3:
                        category = "супа";
                        break;
                    case 4:
                        category = "десерт";
                        break;
                    case 5:
                        category = "предясте";
                        break;
                    case 6:
                        category = "рибно ястие";
                        break;
                    case 7:
                        category = "апетайзер";
                        break;
                    default:
                        break;
                }
                        if (!string.IsNullOrEmpty(category))
                        {
                            recipe.Category = category;
                        }

               
                MCP.Print("Съставки (оставете празно за без промяна): ", "yellow");
                string ingredients = Console.ReadLine();
                if (!string.IsNullOrEmpty(ingredients))
                {
                    recipe.Ingredients = ingredients;
                    UpdateIngredients(recipe, ingredients);
                }

                recipeService.UpdateRecipe(recipe);
                MCP.PrintNL("Рецептата е успешно редактирана!", "green");
            
            
            

                        MCP.Print("Съставки (оставете празно за без промяна): ", "yellow");
                        string ingredients = Console.ReadLine();
                        if (!string.IsNullOrEmpty(ingredients))
                        {
                            recipe.Ingredients = ingredients;
                        }

                        recipeService.UpdateRecipe(recipe);
                        MCP.PrintNL("Рецептата е успешно редактирана!", "green");
                
            }
            catch (Exception)
            {
                MCP.PrintNL("Невалидни данни! Въведете цяло число за ID!", "red");
                
            
        }

        private void UpdateIngredients(Recipe recipe,string ingredients)
        {
            foreach (var ingredientName in ingredients.Split(", ", StringSplitOptions.RemoveEmptyEntries))
            {
                Ingredient ingredient = recipeService.GetIngredientByName(ingredientName);
                if (ingredient != null)
                {
                    MCP.Print($"{ingredient.Name} Грамаж(оставете празно за без промяна): ", "yellow");
                    if (int.TryParse(Console.ReadLine(), out int quantity)) 
                    {
                        recipeService.UpdateRecipeIngredientQuantity(recipe, ingredient, quantity);
                    }
                }
                else
                {
                    MCP.Print($"{ingredient} Грамаж: ", "yellow");
                    int quantity = int.Parse(Console.ReadLine());
                    MCP.Print($"{ingredient} Мярка (например: гр, мл, бр): ", "yellow");
                    string unit = Console.ReadLine();
                    ingredient = new Ingredient { Name = ingredientName };
                    recipeService.AddIngredient(recipe, ingredient, quantity, unit);
                }
            }
        }

        private void AddNewRecipe()
        {
            try
            {
                string food, description, author, category;
                category = "";
                List<int> ingredientIds = new List<int>();
                List<int> quantities = new List<int>();
                List<string> units = new List<string>();

                Console.WriteLine("Име на рецептата: ");
                string food = Console.ReadLine();

                Console.WriteLine("Начин на приготвяне: ");
                string description = Console.ReadLine();

                Console.WriteLine("Автор: ");
                string author = Console.ReadLine();

                Console.WriteLine("Категория: ");
                string category = Console.ReadLine();

                List<int> ingredientIds = new List<int>();
                List<int> quantities = new List<int>();
                List<string> units = new List<string>();

                while (true)

                MCP.PrintNL("Избери една от следните категории: ", "cyan");
                MCP.PrintNL("1. основно ястие", "green");
                MCP.PrintNL("2. салата", "green");
                MCP.PrintNL("3. супа", "green");
                MCP.PrintNL("4. десерт", "green");
                MCP.PrintNL("5. предястие", "green");
                MCP.PrintNL("6. рибно ястие", "green");
                MCP.PrintNL("7. апетайзер", "green");

                int choiceCategory = int.Parse(Console.ReadLine());
                switch (choiceCategory)
                {
                    case 1:
                        category = "основно ястие";
                        break;
                    case 2: category = "салата";
                        break;
                    case 3: category = "супа";
                        break;
                    case 4: category = "десерт";
                        break;
                    case 5:
                        category = "предясте";
                        break;
                    case 6:
                        category = "рибно ястие";
                        break;
                    case 7: category = "апетайзер";
                        break;
                    default:
                        break;
                }
                bool addingIngredients = true;
                while (addingIngredients)
                {
                    Console.WriteLine("Въведете ID на съставката (или оставете празно за край): ");
                    string ingredientInput = Console.ReadLine();
                    if (string.IsNullOrEmpty(ingredientInput))
                    {
                        break;
                    }

                    if (int.TryParse(ingredientInput, out int ingredientId) && recipeService.GetIngredientById(ingredientId) != null)
                    {
                        ingredientIds.Add(ingredientId);

                        Console.WriteLine("Количество: ");
                        if (int.TryParse(Console.ReadLine(), out int quantity))
                        {
                            quantities.Add(quantity);
                        }
                        else
                        {
                            MCP.PrintNL("Невалидно количество!", "red");
                            continue;
                        }

                        Console.WriteLine("Мярка (например: гр, мл, бр): ");
                        string unit = Console.ReadLine();
                        units.Add(unit);
                    }
                    else
                    {
                        MCP.PrintNL("Невалидно ID на съставка!", "red");
                    }
                }

                Recipe recipe = new Recipe(food, description, string.Join(", ", ingredientIds.Select(i => recipeService.GetIngredientById(i).Name)), author, category);
                recipeService.AddRecipe(recipe, ingredientIds, quantities, units);
                MCP.PrintNL("Успешно добавена нова рецепта!", "green");
            }
            catch (Exception ex)
            {
                MCP.PrintNL($"Възникна грешка: {ex.Message}", "red");
            }
        }


        private void SearchRecipeByIngredient()
        {
            MCP.Print("Въведи съставка: ", "cyan");
            var ingredient = Console.ReadLine();

            try
            {
                var search = recipeService.GetRecipeByIngredient(ingredient);

                if (search.Count == 0)
                {
                    MCP.PrintNL("Няма рецепта с този продукт! Може да добавите. :))))", "red");
                    return;
                }
                else   
                {
                    foreach (var recipe in search)
                    {
                        MCP.PrintNL("|" + new string('-', 220) + "|", "yellow");
                        MCP.PrintNL($"|ID: {recipe.Id} | Име: {recipe.Name} | Описание: {recipe.Description} | Съставки: {recipe.Ingredients} | Категория: {recipe.Category} | Автор: {recipe.Author} | Дата на създаване: {recipe.AddDate} |", "yellow");
                    }
                    MCP.PrintNL("|" + new string('-', 220) + "|", "yellow");
                }
              
            }
            catch (Exception)
            {
                MCP.PrintNL("Невалидна съставка!", "red");
            }
        }

        private void SearchRecipeByCategory()
        {
            MCP.PrintNL("Избери една от следните категории: ", "cyan");
            MCP.PrintNL("1. основно ястие", "green");
            MCP.PrintNL("2. салата", "green");
            MCP.PrintNL("3. супа", "green");
            MCP.PrintNL("4. десерт", "green");
            MCP.PrintNL("5. предястие", "green");
            MCP.PrintNL("6. рибно ястие", "green");
            MCP.PrintNL("7. апетайзер", "green");
            string choice = "";
            int choiceCategory = int.Parse(Console.ReadLine());
            switch (choiceCategory)
            {
                case 1:
                    choice = "основно ястие";
                    break;
                case 2:
                    choice = "салата";
                    break;
                case 3:
                    choice = "супа";
                    break;
                case 4:
                    choice = "десерт";
                    break;
                case 5:
                    choice = "предясте";
                    break;
                case 6:
                    choice = "рибно ястие";
                    break;
                case 7:
                    choice = "апетайзер";
                    break;
                default:
                    break;
            }
            

            var recipesByCategory = recipeService.GetRecipeByCategory(choice);
            foreach (var recipe in recipesByCategory)
            {
                MCP.PrintNL("|" + new string('-', 220) + "|", "yellow");
                MCP.PrintNL($"|ID: {recipe.Id} | Име: {recipe.Name} | Описание: {recipe.Description} | Съставки: {recipe.Ingredients} | Категория: {recipe.Category} | Автор: {recipe.Author} | Дата на създаване: {recipe.AddDate} |", "yellow");
            }
            MCP.PrintNL("|" + new string('-', 220) + "|", "yellow");
        }

        public void ShowAllRecipes()
        {
            MCP.PrintNL("Показване на списъка с всички рецепти: ", "red");
            var recipes = recipeService.GetAllRecipes();

            
            foreach (var recipe in recipes)
            {
                MCP.PrintNL("|" + new string('-', 150) + "|", color);
                MCP.PrintNL($"|ID: {recipe.Id} | Name: {recipe.Name} | Description: {recipe.Description} | Author: {recipe.Author} | Date: {recipe.AddDate} | Category: {recipe.Category.Name} | Ingredients: {string.Join(", ", recipe.RecipeIngredients.Select(ri => $"{ri.Ingredient.Name} {ri.Quantity} {ri.Unit.Name}"))} |", color);
            }
            MCP.PrintNL("|" + new string('-', 150) + "|", color);
                MCP.PrintNL("|"  + new string('-', 220) + "|", "yellow");
                MCP.PrintNL($"|ID: {recipe.Id} | Име: {recipe.Name} | Описание: {recipe.Description} | Съставки: {recipe.Ingredients} | Категория: {recipe.Category} | Автор: {recipe.Author} | Дата на създаване: {recipe.AddDate} |", "yellow");
            }
            MCP.PrintNL("|" + new string('-', 220) + "|", "yellow");

        }

        private int AddUnit()
        {
            int unitId;
            Console.WriteLine("-----Избор на мярка------");
            Console.WriteLine("1. Избор от наличните мярки");
            Console.WriteLine("2. Добавяне на нова мярка");
            Console.Write("Изберете опция(1-2): ");
            int choiceUnit = int.Parse(Console.ReadLine());
            if (choiceUnit == 1)
            {
                var units = recipeService.GetAllUnits();
                foreach (var unit in units)
                {
                    Console.WriteLine($"{unit.Id} {unit.Name}");
                }
                Console.Write("Изберете Id на мярката: ");
                unitId = int.Parse(Console.ReadLine());
            }
            else
            {
                Console.Write("Въведете име на мярката: ");
                string unitName = Console.ReadLine();
                Unit unit = new Unit() { Name = unitName };
                recipeService.AddUnit(unit);
                unitId = unit.Id;
            }
            return unitId;
        }
    }
}
