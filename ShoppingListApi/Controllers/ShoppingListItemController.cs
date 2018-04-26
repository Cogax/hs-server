using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ShoppingListApi.Models;

namespace ShoppingListApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class ShoppingListItemController : Controller
    {
        private readonly ShoppingListContext _context;

        public ShoppingListItemController(ShoppingListContext context)
        {
            _context = context;

            if(_context.ShoppingListItems.Count() == 0) {
                _context.ShoppingListItems.Add(new ShoppingListItem { Label = "Banane" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<ShoppingListItem> GetAll()
        {
            return _context.ShoppingListItems.ToList();
        }

        [HttpGet("{id}", Name = "GetItem")]
        public IActionResult GetById(long id)
        {
            var item = _context.ShoppingListItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ShoppingListItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.ShoppingListItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetItem", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] ShoppingListItem item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var dbItem = _context.ShoppingListItems.FirstOrDefault(t => t.Id == id);
            if (dbItem == null)
            {
                return NotFound();
            }

            dbItem.IsComplete = item.IsComplete;
            // dbItem.Label = item.Label; not yet possible

            _context.ShoppingListItems.Update(dbItem);
            _context.SaveChanges();
            return new ObjectResult(item);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _context.ShoppingListItems.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.ShoppingListItems.Remove(todo);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
