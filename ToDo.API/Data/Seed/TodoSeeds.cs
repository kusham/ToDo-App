using Microsoft.EntityFrameworkCore;
using ToDo.API.Models;

namespace ToDo.API.Data.Seed
{
    public class TodoSeeds
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // Seed User data
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = "1",
                    FirstName = "John",
                    LastName = "Doe",
                    //Username = "john.doe"
                },
                new User
                {
                    Id = "2",
                    FirstName = "Jane",
                    LastName = "Doe",
                    //Username = "jane.doe"
                }
            );

            modelBuilder.Entity<Todo>().HasData(
               new Todo
                {
                    Id = Guid.NewGuid(),
                    Title = "Learn ASP.NET Core",
                    Description = "Learn ASP.NET Core with EF Core",
                    Status = TodoStatus.Pending,
                    UserId = "1"
                },
               new Todo
                {
                    Id = Guid.NewGuid(),
                    Title = "Learn Angular",
                    Description = "Learn Angular with ASP.NET Core",
                    Status = TodoStatus.Pending,
                    UserId = "1"
               },
               new Todo
                {
                    Id = Guid.NewGuid(),
                    Title = "Learn React",
                    Description = "Learn React with ASP.NET Core",
                    Status = TodoStatus.Completed,
                    UserId = "1"
               },
               new Todo
               {
                    Id = Guid.NewGuid(),
                    Title = "Learn Vue",
                    Description = "Learn Vue with ASP.NET Core",
                    Status = TodoStatus.Pending,
                    UserId = "2"
               }
               );
        }
    }
}
