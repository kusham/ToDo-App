using ToDo.API.Models;

namespace ToDo.API.Dtos
{
    public class TodoDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public TodoStatus Status { get; set; }
    }
}
