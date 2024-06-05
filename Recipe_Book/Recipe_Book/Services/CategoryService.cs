﻿using Recipe_Book.Data;
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

        public CategoryService()
        {
            _context = new DBConnect();
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