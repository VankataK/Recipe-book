using Recipe_Book.Data;
using Recipe_Book.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Book.Services
{
    internal class CategoryService
    {
        private readonly DBConnect _context;
        private readonly List<string> categoriesNames = new List<string> { "Предястие", "Салата", "Основно", "Десерт", "Закуска", "Тестено изделие", "Разядка"};
        public CategoryService()
        {
            _context = new DBConnect();
        }

        public void AddCategoriesToDB()
        {
            foreach (var categoryName in categoriesNames)
            {
                if (!_context.Categories.Select(c => c.Name).Contains(categoryName))
                {
                    _context.Categories.Add(new Category { Id = categoriesNames.IndexOf(categoryName), Name = categoryName });
                }
            }
            _context.SaveChanges();
        }
        public Category GetCategoryById(int categoryId)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == categoryId);
        }
        public List<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }
    }
}
