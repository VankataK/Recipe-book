﻿using Recipe_Book.Data;
using Recipe_Book.Views;

namespace Recipe_Book.Controllers
{
    public class Controller
    {
        public void Start()
        {
            using (var contect = new DBConnect())
            {
                var display = new Display();
                display.ShowMenu();
            }
        }
    }
}
