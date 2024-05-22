using Recipe_Book.Services;
using CustomPrint;

namespace Recipe_Book.Views
{
    public class Display
    {
        private readonly RecipeService recipeService;

        public Display(RecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        public async Task ShowMenu()
        {
            bool exit = false;
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

            }
        }
    }
}
