using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Recipe_Book.Data;
using Recipe_Book.Models;
using Recipe_Book.Services;
using Recipe_Book.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Recipe_Book.Controllers
{
    public class Controller
    {
        public void Start()
        {
            using (var contect = new DBConnect())
            {
                var recipeService = new RecipeService(contect);
                var display = new Display(recipeService);
                display.ShowMenu();
            }
        }
    }
}
