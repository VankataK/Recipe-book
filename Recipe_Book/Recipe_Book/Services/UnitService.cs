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

        public UnitService()
        {
            _context = new DBConnect();
        }

        public List<Unit> GetAllUnits()
        {
            return _context.Units.ToList();
        }

    }
}
