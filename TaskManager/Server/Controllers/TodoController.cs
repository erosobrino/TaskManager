using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TaskManager.Server.Infrastructure;
using TaskManager.Shared;

namespace TaskManager.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoDbContext _todoDbContext;

        public TodoController(TodoDbContext todoDbContext) => _todoDbContext = todoDbContext;

        [HttpGet]
        public IActionResult Finished()
        {
            var query = _todoDbContext.Todos.Where(x => x.Done == true);

            return Ok(query.ToList());
        }

        [HttpGet]
        public IActionResult Pending()
        {
            var query = _todoDbContext.Todos.Where(x => x.Done == false);

            return Ok(query.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var todo = _todoDbContext.Todos.FirstOrDefault(x => x.Id == id);

            if (todo is null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var todo = _todoDbContext.Todos.FirstOrDefault(x => x.Id == id);

            if (todo is null)
            {
                return NotFound();
            }

            _ = _todoDbContext.Todos.Remove(todo);
            _ = _todoDbContext.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IActionResult Update(Todo newTodo)
        {
            if (newTodo is null)
            {
                return BadRequest();
            }

            var now = DateTime.Now;

            if (newTodo.Id == Guid.Empty)
            {
                newTodo.Timestamp = now;
                _ = _todoDbContext.Todos.Add(newTodo);
            }
            else
            {
                var dbTodo = _todoDbContext.Todos.FirstOrDefault(x => x.Id == newTodo.Id);
                if (dbTodo is null)
                {
                    newTodo.Timestamp = now;
                    _ = _todoDbContext.Todos.Add(newTodo);
                }
                else
                {
                    if (dbTodo.Timestamp > newTodo.Timestamp)
                    {
                        return BadRequest("The entity is not updated");
                    }

                    dbTodo.Name = newTodo.Name;
                    dbTodo.Description = newTodo.Description;
                    dbTodo.Done = newTodo.Done;
                    dbTodo.Timestamp = now;
                }
            }
            _ = _todoDbContext.SaveChanges();

            return Ok();
        }
    }
}
