using Comman;
using ToDo.API.Models;

namespace ToDo.API.Serives
{
    public interface ITodoService : IService<Todo>
    {
        Task<IEnumerable<Todo>> GetAllBySearchAsync(string userId, string search, TodoStatus? status, DateTime? dueDate);
        Task<IEnumerable<Todo>> GetAllByUserIdAsync(string userId);
    }
}
