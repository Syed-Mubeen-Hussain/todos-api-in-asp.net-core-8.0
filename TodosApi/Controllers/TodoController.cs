using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.FileProviders;
using TodosApi.DTO;
using TodosApi.Service;

namespace TodosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet("GetAllTodos")]
        public async Task<IActionResult> GetTodos()
        {
            var data = await _todoService.GetTodos();
            if (data.Count == 0)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpGet("GetTodoById/{id}")]
        public async Task<IActionResult> GetTodoById(int id)
        {
            var data = await _todoService.GetTodoById(id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpPost("AddTodo")]
        public async Task<IActionResult> AddTodo(TodoDTO todo)
        {
            var data = await _todoService.CreateTodo(todo);
            return Ok(data);
        }

        [HttpPut("UpdateTodo")]
        public async Task<IActionResult> UpdateTodo(int id, TodoDTO todo)
        {
            var data = await _todoService.UpdateTodo(id, todo);
            return Ok(data);
        }

        [HttpDelete("DeleteTodo")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var data = await _todoService.RemoveTodo(id);
            return Ok(data);
        }
    }
}
