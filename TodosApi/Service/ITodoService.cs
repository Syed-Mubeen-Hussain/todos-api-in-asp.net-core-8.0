using TodosApi.DTO;
using TodosApi.Helper;

namespace TodosApi.Service
{
    public interface ITodoService
    {
        public Task<List<TodoDTO>> GetTodos();
        public Task<TodoDTO> GetTodoById(int id);
        public Task<APIResponse> CreateTodo(TodoDTO todo);
        public Task<APIResponse> RemoveTodo(int id);
        public Task<APIResponse> UpdateTodo(int id, TodoDTO todo);
    }
}
