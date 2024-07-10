using Comman;
using ToDo.API.Models;

namespace ToDo.API.Repository
{
    public interface ITodoRepository : IRepository<Todo>
    {
        Task<IEnumerable<Todo>> GetAllBySearchAsync(string userId, string search, TodoStatus? status, DateTime? dueDate);
        Task<IEnumerable<Todo>> GetTodosByUserIdAsync(string userId);
    }
}
