using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ToDo.API.Models
{
    public class User : IdentityUser
    {
        public ICollection<Todo> Todos { get; set; } = [];
    }
}
