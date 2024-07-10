using Comman;
using Microsoft.EntityFrameworkCore;
using ToDo.API.Data;
using ToDo.API.Models;

namespace ToDo.API.Repository
{
    public class TodoRepository : GenericRepository<Todo>, ITodoRepository
    {
        private readonly ApplicationDbContext _context;
        public TodoRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Todo>> GetTodosByUserIdAsync(string userId)
        {
            return await _context.Todos.Where(t => t.UserId == userId.ToString()).ToListAsync();
        }

        public async Task<IEnumerable<Todo>> GetAllBySearchAsync(string userId, string search, 
            TodoStatus? status, DateTime? dueDate)
        {
            var query = _context.Todos.Where(t => t.UserId == userId);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t => t.Title.Contains(search) || t.Description.Contains(search));
            }

            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }

            if (dueDate.HasValue)
            {
                query = query.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == dueDate.Value.Date);
            }

            return await query.ToListAsync();
        }
    }
}
