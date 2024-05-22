using Recipe_Book.Services;
using CustomPrint;
using Recipe_Book.Models;
using System.Threading.Channels;

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
            throw new NotImplementedException();
        }

        private void UpdateRecipe()
        {
            throw new NotImplementedException();
        }

        private void AddNewRecipe()
        {
            Recipe recipe = new Recipe("Burger", "Slagash pitkata i kufteto", "kufte, pitka, domat","Az","Osnovno");
            recipeService.AddRecipe(recipe, new List<int> { 1, 2, 3 }, new List<int> { 1, 2, 100 }, new List<string> { "broika", "broika", "kilo" });

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
            var list = recipeService.GetAllRecipes();

        }

        
    }
}
