using Recipe_Book.Data;
using Recipe_Book.Models;

namespace Recipe_Book.Services
{
    public class CategoryService
    {
        private readonly DBConnect _context;
        private readonly List<string> categoriesNames = new List<string> { "Предястие", "Салата", "Основно", "Десерт", "Закуска", "Тестено изделие", "Разядка"};
        public CategoryService()
        {
            _context = new DBConnect();
        }
        public CategoryService(DBConnect db)
        {
            _context = db;
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
