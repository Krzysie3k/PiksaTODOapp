using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PIKSA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private static List<TodoItem> todos = new();

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = todos.FirstOrDefault(t => t.Id == id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoItem newItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            newItem.Id = todos.Count + 1;
            todos.Add(newItem);
            return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TodoItem updatedItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = todos.FirstOrDefault(t => t.Id == id);
            if (item == null) return NotFound();

            item.Title = updatedItem.Title;
            item.IsDone = updatedItem.IsDone;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = todos.FirstOrDefault(t => t.Id == id);
            if (item == null) return NotFound();

            todos.Remove(item);
            return NoContent();
        }
    }

    public class TodoItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be 3-100 characters")]
        public string Title { get; set; }

        public bool IsDone { get; set; } = false;
    }
}
