using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;

namespace ToDo.API.Models
{
    public class Todo
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Title length can't be more than 100.")]
        public required string Title { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Description length can't be more than 500.")]
        public required string Description { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? DueDate { get; set; }
        public TodoStatus Status { get; set; } = TodoStatus.Pending;
        [Required]
        public required string UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
    }

    public enum TodoStatus
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2
    }
}
