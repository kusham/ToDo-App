using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ToDo.API.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public ICollection<Todo> Todos { get; set; } = [];

        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }

}
