using Recipe_Book.Services;
using CustomPrint;
using Recipe_Book.Models;
using System.Threading.Channels;
using System.Runtime.CompilerServices;
using System.Text;

namespace Recipe_Book.Views
{

    public class Display
    {

        private readonly RecipeService recipeService;

        public Display(RecipeService recipeService)
        {
            this.recipeService = recipeService;
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
                MCP.PrintNL(" 2. ТЪРСИ РЕЦЕПТА ПО КАТЕГОРИЯ", "cyan");
                MCP.PrintNL(" 3. ТЪРСИ РЕЦЕПТА ПО ПРОДУКТ", "cyan");
                MCP.PrintNL(" 4. ДОБАВИ НОВА РЕЦЕПТА", "cyan");
                MCP.PrintNL(" 5. РЕДАКТИРАЙ РЕЦЕПТА", "cyan");
                MCP.PrintNL(" 6. ИЗТРИЙ РЕЦЕПТА", "cyan");
                MCP.PrintNL(" 7. ИЗХОД", "cyan");

                MCP.Print("Въведете номер на команда (1-7): ", "cyan");

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
                        SearchRecipeByProduct();
                        break;

                    case 4:
                        AddNewRecipe();
                        break;

                    case 5:
                        UpdateRecipe();
                        break;

                    case 6:
                        DeleteRecipe();
                        break;
                    case 7:
                        exit = true;
                        break;
                    default:
                        MCP.PrintNL("Невалидна команда!","red");                     
                        break;

                }
            }
        }

        private void DeleteRecipe()
        {
            Console.OutputEncoding = Encoding.UTF8;
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

        private void UpdateRecipe()
        {
            Console.OutputEncoding = Encoding.UTF8;
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

                MCP.Print("Категория: (оставете празно за без промяна) ", "yellow");
                string category = Console.ReadLine();
                if (!string.IsNullOrEmpty(category))
                {
                    recipe.Category = category;
                }

               
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
        }

        private void AddNewRecipe()
        {
            Console.OutputEncoding = Encoding.UTF8;
            try
            {
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

                Recipe recipe = new Recipe(food, description, string.Join(", ", ingredientIds), author, category);
                recipeService.AddRecipe(recipe, ingredientIds, quantities, units);
                MCP.PrintNL("Успешно добавена нова рецепта!", "green");
            }
            catch (Exception ex)
            {
                MCP.PrintNL($"Възникна грешка: {ex.Message}", "red");
            }
        }


        private void SearchRecipeByProduct()
        {
            throw new NotImplementedException();
        }

        private void SearchRecipeByCategory()
        {
            throw new NotImplementedException();
        }

        public void ShowAllRecipes()
        {
            Console.OutputEncoding = Encoding.UTF8;
            MCP.PrintNL("Показване на списъка с всички рецепти: ", "red");
            var recipes = recipeService.GetAllRecipes();

            
            foreach (var recipe in recipes)
            {
                MCP.PrintNL("|"  + new string('-', 150) + "|", "yellow");
                MCP.PrintNL($"|ID: {recipe.Id} | Name: {recipe.Name} | Description: {recipe.Description} | Ingredients: {recipe.Ingredients} | Author: {recipe.Author} | Data: {recipe.AddDate} |", "yellow");
            }
            MCP.PrintNL("|" + new string('-', 150) + "|", "yellow");

        }

        
    }
}
