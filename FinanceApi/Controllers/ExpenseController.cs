using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class ExpenseController : Controller
    {
        private readonly FinanceContext _context;

        public ExpenseController(FinanceContext context)
        {
            _context = context;

            if (_context.Expenses.Count() == 0)
            {
                _context.Expenses.Add(new Expense { Label = "Banane", Amount = 100.00 });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<Expense> GetAll()
        {
            return _context.Expenses.ToList();
        }

        [HttpGet("{id}", Name = "GetItem")]
        public IActionResult GetById(long id)
        {
            var item = _context.Expenses.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Expense item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.Expenses.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetItem", new { id = item.Id }, item);
        }
    }
}
