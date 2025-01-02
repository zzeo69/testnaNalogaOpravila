using Domain.Entities;
using Domain.Interfaces;
using Common.Logger;

namespace Application.Services
{
    public class TaskService
    {
        private readonly ITaskOperations _taskOperations;
        private readonly ILogger _logger;

        public TaskService(ITaskOperations taskOperations, ILogger logger)
        {
            _taskOperations = taskOperations;
            _logger = logger;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            _logger.LogInfo("Getting all tasks.");
            return await _taskOperations.GetAllAsync();
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int id)
        {
            _logger.LogInfo($"Getting task with ID: {id}.");
            return await _taskOperations.GetByIdAsync(id);
        }

        public async Task AddTaskAsync(TaskItem task)
        {
            task.CreatedDate = DateTime.Now;
            task.UpdatedDate = DateTime.Now;

            if (!Enum.IsDefined(typeof(PriorityLevelEnum), task.Priority))
            {
                _logger.LogError($"Invalid Priority: {task.Priority}");
                throw new Exception("Invalid Priority.");
            }

            _logger.LogInfo($"Adding new task with Title: {task.Title}.");
            await _taskOperations.AddAsync(task);
        }

        public async Task UpdateTaskAsync(int id, TaskItem task)
        {
            task.UpdatedDate = DateTime.Now;

            if (!Enum.IsDefined(typeof(PriorityLevelEnum), task.Priority))
            {
                _logger.LogError($"Invalid Priority: {task.Priority}");
                throw new Exception("Invalid Priority.");
            }

            _logger.LogInfo($"Updating task with ID: {task.ID}.");
            await _taskOperations.UpdateAsync(id, task);
        }

        public async Task DeleteTaskAsync(int id)
        {
            _logger.LogInfo($"Deleting task with ID: {id}.");
            await _taskOperations.DeleteAsync(id);
        }
    }
}
