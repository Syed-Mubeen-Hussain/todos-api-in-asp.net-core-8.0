using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Formats.Tar;
using System.Globalization;
using TodosApi.Data;
using TodosApi.DTO;
using TodosApi.Helper;
using TodosApi.Models;
using TodosApi.Service;

namespace TodosApi.Container
{
    public class TodoService : ITodoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<TodoDTO> _validator;

        public TodoService(ApplicationDbContext context, IValidator<TodoDTO> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<APIResponse> CreateTodo(TodoDTO todo)
        {
            APIResponse response = new APIResponse();

            var res = _validator.Validate(todo);

            if (!res.IsValid)
            {
                response.StatusCode = 400;
                response.ErrorMessage = res.Errors.FirstOrDefault().ErrorMessage;
                return response;
            }

            try
            {
                var newTodo = new Models.Todo
                {
                    Title = todo.Title,
                    Description = todo.Description,
                    IsCompleted = todo.IsCompleted,
                    CreatedDate = DateTime.Now,
                    DueDate = ConvertStringIntoDate(todo.DueDate),
                    Priority = todo.Priority
                };
                await _context.Todos.AddAsync(newTodo);
                await _context.SaveChangesAsync();

                response.StatusCode = 201;
                response.Result = $"New todo created with id : {newTodo.Id}";
            }
            catch (Exception ex)
            {
                response.StatusCode = 400;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public async Task<TodoDTO> GetTodoById(int id)
        {
            var todo = await _context.Todos.FirstOrDefaultAsync(x => x.Id == id);
            if (todo == null)
            {
                return null;
            }
            var response = new TodoDTO()
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
                CreatedDate = todo.CreatedDate.ToString("dd/MM/yyyy"),
                DueDate = todo.DueDate.ToString("dd/MM/yyyy"),
                Priority = todo.Priority
            };
            return response;
        }

        public async Task<List<TodoDTO>> GetTodos()
        {
            var response = new List<TodoDTO>();
            var todos = await _context.Todos.ToListAsync();
            if (todos != null && todos.Count > 0)
            {
                response = todos.Select(x => new TodoDTO
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    CreatedDate = x.CreatedDate.ToString("dd/MM/yyyy"),
                    DueDate = x.DueDate.ToString("dd/MM/yyyy"),
                    IsCompleted = x.IsCompleted,
                    Priority = x.Priority
                }).ToList();
            }
            return response;
        }

        public async Task<APIResponse> RemoveTodo(int id)
        {
            APIResponse response = new APIResponse();
            
            if (id == 0)
            {
                response.StatusCode = 400;
                response.ErrorMessage = "Id is required";
                return response;
            }

            try
            {
                var todo = await _context.Todos.FirstOrDefaultAsync(x => x.Id == id);
                if (todo == null)
                {
                    response.StatusCode = 404;
                    response.ErrorMessage = "Todo not found";
                    return response;
                }

                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();

                response.StatusCode = 200;
                response.Result = $"Removed the todo with id : {todo.Id}";

            }
            catch (Exception ex)
            {
                response.StatusCode = 400;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> UpdateTodo(int id, TodoDTO todo)
        {
            APIResponse response = new APIResponse();

            if (id == 0)
            {
                response.StatusCode = 400;
                response.ErrorMessage = "Id is required";
                return response;
            }

            var res = _validator.Validate(todo);

            if (!res.IsValid)
            {
                response.StatusCode = 400;
                response.ErrorMessage = res.Errors.FirstOrDefault().ErrorMessage;
                return response;
            }

            try
            {
                var checkTodo = await _context.Todos.FirstOrDefaultAsync(x => x.Id == id);

                if (checkTodo == null)
                {
                    response.StatusCode = 404;
                    response.ErrorMessage = "Todo not found";
                    return response;
                }

                checkTodo.Title = todo.Title;
                checkTodo.Description = todo.Description;
                checkTodo.DueDate = ConvertStringIntoDate(todo.DueDate);
                checkTodo.IsCompleted = todo.IsCompleted;
                checkTodo.Priority = todo.Priority;
                await _context.SaveChangesAsync();

                response.StatusCode = 200;
                response.Result = $"Updated the todo with id : {todo.Id}";
            }
            catch (Exception ex)
            {
                response.StatusCode = 400;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public DateTime ConvertStringIntoDate(string dateString)
        {
            DateTime date = DateTime.ParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return date;
        }
    }
}
