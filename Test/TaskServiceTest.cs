using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Test
{
    public class TaskServiceTest
    {
        private readonly Mock<ITaskOperations> _mockOperations;
        private readonly Mock<Common.Logger.ILogger> _mockLogger;
        private readonly TaskService _taskService;

        public TaskServiceTest()
        {
            _mockOperations = new Mock<ITaskOperations>();
            _mockLogger = new Mock<Common.Logger.ILogger>();
            _taskService = new TaskService(_mockOperations.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllTasksAsync_ReturnsAllTasks()
        {
            _mockOperations.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<TaskItem>
            {
                new TaskItem { ID = 1, Title = "Prva naloga", Description = "To je prva naloga.", DueDate = DateTime.Now.AddDays(1), Priority = PriorityLevelEnum.Medium },
                new TaskItem { ID = 2, Title = "Druga naloga", Description = "To je druga naloga.", DueDate = DateTime.Now.AddDays(5), Priority = PriorityLevelEnum.High },
            });

            var result = await _taskService.GetAllTasksAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockLogger.Verify(l => l.LogInfo("Getting all tasks."), Times.Once);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ReturnsTask()
        {
            int taskID = 1;
            var expectedTask = new TaskItem { ID = 1, Title = "Prva naloga", Description = "To je prva naloga.", DueDate = DateTime.Now.AddDays(1), Priority = PriorityLevelEnum.Medium };

            _mockOperations.Setup(s => s.GetByIdAsync(taskID)).ReturnsAsync(expectedTask);

            var result = await _taskService.GetTaskByIdAsync(taskID);

            Assert.NotNull(result);
            Assert.Equal(taskID, result.ID);
            Assert.Equal(expectedTask, result);
            _mockLogger.Verify(l => l.LogInfo($"Getting task with ID: {taskID}."), Times.Once);
        }

        [Fact]
        public async Task AddTaskAsync_CorrectDefaults()
        {
            var insertedTask = new TaskItem { ID = 1, Title = "Prva naloga", DueDate = DateTime.Now.AddDays(1), Priority = PriorityLevelEnum.Medium };
            TaskItem? capturedTask = null;

            _mockOperations.Setup(s => s.AddAsync(insertedTask)).Callback<TaskItem>(c => capturedTask = c).Returns(Task.CompletedTask);

            await _taskService.AddTaskAsync(insertedTask);

            Assert.NotNull(capturedTask);
            Assert.Equal(capturedTask.ID, insertedTask.ID);
            Assert.Equal(string.Empty, capturedTask.Description);
            Assert.Equal(false, capturedTask.IsCompleted);
            _mockLogger.Verify(l => l.LogInfo($"Adding new task with Title: {capturedTask.Title}."), Times.Once);
        }

        [Fact]
        public async Task AddTaskAsync_InvalidPriorityLevel()
        {
            var invalidTask = new TaskItem { ID = 1, Title = "Prva naloga", DueDate = DateTime.Now.AddDays(1), Priority = (PriorityLevelEnum)10 };

            _mockOperations.Setup(s => s.AddAsync(invalidTask)).Returns(Task.CompletedTask);

            var exception = await Assert.ThrowsAsync<Exception>(() => _taskService.AddTaskAsync(invalidTask));

            Assert.Equal("Invalid Priority.", exception.Message);
            _mockOperations.Verify(o => o.AddAsync(It.IsAny<TaskItem>()), Times.Never);
            _mockLogger.Verify(l => l.LogError($"Invalid Priority: {invalidTask.Priority}"), Times.Once);
        }

        [Fact]
        public async Task UpdateTaskAsync_CorrectUpdateDate()
        {
            var taskID = 3;
            var initDate = DateTime.Now;
            var updatedTask = new TaskItem { ID = 3, Title = "Prva naloga", DueDate = DateTime.Now.AddDays(1), Priority = PriorityLevelEnum.Medium, CreatedDate = initDate, UpdatedDate = initDate };

            _mockOperations.Setup(s => s.GetByIdAsync(taskID)).ReturnsAsync(updatedTask);
            _mockOperations.Setup(s => s.UpdateAsync(taskID, updatedTask)).Returns(Task.CompletedTask);

            await _taskService.UpdateTaskAsync(taskID, updatedTask);

            Assert.NotEqual(initDate, updatedTask.UpdatedDate);
            Assert.True(updatedTask.UpdatedDate <= DateTime.Now && updatedTask.UpdatedDate > updatedTask.CreatedDate);
            _mockLogger.Verify(l => l.LogInfo($"Updating task with ID: {taskID}."), Times.Once);
        }

        [Fact]
        public async Task UpdateTaskAsync_InvalidPriorityLevel()
        {
            var taskID = 1;
            var invalidTask = new TaskItem { ID = 1, Title = "Prva naloga", DueDate = DateTime.Now.AddDays(1), Priority = (PriorityLevelEnum)10 };

            _mockOperations.Setup(s => s.UpdateAsync(taskID, invalidTask)).Returns(Task.CompletedTask);

            var exception = await Assert.ThrowsAsync<Exception>(() => _taskService.UpdateTaskAsync(taskID, invalidTask));

            Assert.Equal("Invalid Priority.", exception.Message);
            _mockOperations.Verify(o => o.UpdateAsync(It.IsAny<int>(), It.IsAny<TaskItem>()), Times.Never);
            _mockLogger.Verify(l => l.LogError($"Invalid Priority: {invalidTask.Priority}"), Times.Once);
        }

        [Fact]
        public async Task DeleteTaskAsync_ConfirmDelete()
        {
            int taskID = 1;

            await _taskService.DeleteTaskAsync(taskID);

            _mockOperations.Verify(o => o.DeleteAsync(taskID), Times.Once);
            _mockLogger.Verify(l => l.LogInfo($"Deleting task with ID: {taskID}."), Times.Once);
        }
    }
}