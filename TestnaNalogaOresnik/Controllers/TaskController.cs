using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _taskService;
        private readonly Common.Logger.ILogger _logger;

        public TaskController(TaskService taskService, Common.Logger.ILogger logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet("tasks")]
        public async Task<IActionResult> GetTasks()
        {
            try
            {
                var tasks = await _taskService.GetAllTasksAsync();

                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all tasks: {ex.Message}");
                return StatusCode(500, "An error occurred.");
            }
        }

        [HttpGet("tasks/{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);

                if (task == null) return NotFound();

                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting task with ID {id}: {ex.Message}");
                return StatusCode(500, "An error occurred.");
            }
        }

        [HttpPost("tasks")]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
        {
            try
            {
                await _taskService.AddTaskAsync(task);

                return CreatedAtAction(nameof(GetTask), new { id = task.ID ?? -1 }, task);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating task with Title {task.Title}: {ex.Message}");
                return StatusCode(500, "An error occurred.");
            }
        }

        [HttpPut("tasks/{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem task)
        {
            try
            {
                var existingTask = await _taskService.GetTaskByIdAsync(id);
                if (existingTask == null) return NotFound();

                task.ID = id;
                await _taskService.UpdateTaskAsync(id, task);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating task with ID {task.ID}: {ex.Message}");
                return StatusCode(500, "An error occurred.");
            }
        }

        [HttpDelete("tasks/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var existingTask = await _taskService.GetTaskByIdAsync(id);
                if (existingTask == null) return NotFound();

                await _taskService.DeleteTaskAsync(id);

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error deleting task with ID {id}: {ex.Message}");
                return StatusCode(500, "An error occurred.");
            }
        }
    }
}
