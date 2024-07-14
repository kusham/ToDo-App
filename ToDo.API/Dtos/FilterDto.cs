using ToDo.API.Models;

namespace ToDo.API.Dtos
{
    public class FilterDto
    {
        public required string Text { get; set; }
        public TodoStatus? Status { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
