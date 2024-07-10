using Comman;
using ToDo.API.Models;
using ToDo.API.Repository;

namespace ToDo.API.Serives
{
    public class TodoService : GenericService<Todo>, ITodoService
    {
        private readonly ITodoRepository _repository;
        public TodoService(ITodoRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public Task<IEnumerable<Todo>> GetAllBySearchAsync(string userId, string search, TodoStatus? status, DateTime? dueDate)
        {
            return _repository.GetAllBySearchAsync(userId, search, status, dueDate);
        }

        public Task<IEnumerable<Todo>> GetAllByUserIdAsync(string userId)
        {
            return _repository.GetTodosByUserIdAsync(userId);
        }
    }
}
