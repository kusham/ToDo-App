using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToDo.API.Dtos;
using ToDo.API.Models;
using ToDo.API.Serives;

namespace ToDo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly ILogger<TodoController> _logger;

        public TodoController(ITodoService service, ILogger<TodoController> logger)
        {
            this._todoService = service;
            this._logger = logger;
        }

        // GET: api/Todo/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Todo>>> GetAll()
        {
            try
            {
                // Get the authenticated user's ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    _logger.LogWarning("User not authenticated.");
                    return Unauthorized("User not authenticated.");
                }
                var todos = await _todoService.GetAllByUserIdAsync(userId);
                //var todos = await _todoService.GetAllAsync();
                return Ok(todos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all todos.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        // GET: api/Todo/:id
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetById(Guid id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var todo = await _todoService.GetByIdAsync(id);
                if (todo == null)
                {
                    _logger.LogWarning("Todo with id {TodoId} not found.", id);
                    return NotFound();
                }
                if (todo.UserId != userId)
                {
                    _logger.LogWarning("User with id {UserId} does not own todo with id {TodoId}.", userId, id);
                    return Forbid(); // User does not own this todo
                }

                return Ok(todo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving todo with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        // POST: api/Todo
        [HttpPost]
        public async Task<ActionResult<Todo>> Create(Todo todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (todo.UserId != userId)
                {
                    _logger.LogWarning("Authenticated User and the ID given does not match");
                    return Forbid(); 
                }
                todo.UserId = userId;
                await _todoService.CreateAsync(todo);
                return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new todo.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        // PUT: api/Todo/:id
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Todo todo)
        {
            if (id != todo.Id)
            {
                _logger.LogWarning("Todo id mismatch.");
                return BadRequest();
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var existingTodo = await _todoService.GetByIdAsync(id);

                if (existingTodo == null)
                {
                    _logger.LogWarning("Todo with id {TodoId} not found.", id);
                    return NotFound();
                }

                if (existingTodo.UserId != userId)
                {
                    _logger.LogWarning("User with id {UserId} does not own todo with id {TodoId}.", userId, id);
                    return Forbid(); // User does not own this todo
                }
                existingTodo.Title = todo.Title;
                existingTodo.Description = todo.Description;
                existingTodo.DueDate = todo.DueDate;
                existingTodo.Status = todo.Status;

                await _todoService.UpdateAsync(existingTodo);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating todo with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        // DELETE: api/Todo/:id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var todo = await _todoService.GetByIdAsync(id);

                if (todo == null)
                {
                    _logger.LogWarning("Todo with id {TodoId} not found.", id);
                    return NotFound();
                }

                if (todo.UserId != userId)
                {
                    _logger.LogWarning("User with id {UserId} does not own todo with id {TodoId}.", userId, id);
                    return Forbid(); // User does not own this todo
                }


                await _todoService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting todo with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        // GET: api/Todo/filter
        [HttpPost("filter")]
        public async Task<IActionResult> GetAll([FromBody] FilterDto filter)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var todos = await _todoService.GetAllBySearchAsync(userId, filter.Text, filter.Status, filter.DueDate);
                return Ok(todos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving todos.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

    }
}
