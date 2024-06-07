using Microsoft.EntityFrameworkCore;
using Recipe_Book.Data;
using Recipe_Book.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Book.Services
{
    public class UnitService
    {
        private readonly DBConnect _context;
        private readonly List<string> UnitsNames = new List<string>(){ "броя", "гр", "кг", "л", "мл", "чаши", "супени лъжици", "чаени лъжици"};

        public UnitService()
        {
            _context = new DBConnect();
        }

        public void AddUnitsToDB()
        {
            foreach (var unitName in UnitsNames)
            {
                if (!_context.Units.Select(u=> u.Name).Contains(unitName))
                {
                    _context.Units.Add(new Unit { Name = unitName });
                }
            }
            _context.SaveChanges();
        }
        public List<Unit> GetAllUnits()
        {
            return _context.Units.ToList();
        }

    }
}
