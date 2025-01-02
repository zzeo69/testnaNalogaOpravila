using System.Net;
using System.Net.Http.Json;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Test
{
    public class TaskControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly Mock<ITaskOperations> _mockOperations;

        public TaskControllerTest(WebApplicationFactory<Program> factory)
        {
            _mockOperations = new Mock<ITaskOperations>();
            _httpClient = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(s =>
                {
                    s.AddSingleton(_mockOperations.Object);
                });
            }).CreateClient();
        }

        [Fact]
        public async Task GetTasks_ReturnsOk()
        {
            _mockOperations.Reset();
            
            var response = await _httpClient.GetAsync("/api/Task/tasks");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetTasks_ReturnsException()
        {
            _mockOperations.Reset();
            _mockOperations.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception("Test exception."));

            var response = await _httpClient.GetAsync("/api/Task/tasks");

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task GetTask_ReturnsTask()
        {
            var taskID = 1;
            var expectedTask = new TaskItem { ID = 1, Title = "Prva naloga", Description = "To je prva naloga.", DueDate = DateTime.Now.AddDays(1), Priority = PriorityLevelEnum.Medium };

            _mockOperations.Reset();
            _mockOperations.Setup(s => s.GetByIdAsync(taskID)).ReturnsAsync(expectedTask);

            var response = await _httpClient.GetAsync($"/api/Task/tasks/{taskID}");
            response.EnsureSuccessStatusCode();

            var task = await response.Content.ReadFromJsonAsync<TaskItem>();
            Assert.NotNull(task);
            Assert.Equal(task.ID, taskID);
        }

        [Fact]
        public async Task GetTask_ReturnsNoTaskFound()
        {
            var taskID = 1;

            _mockOperations.Reset();
            _mockOperations.Setup(s => s.GetByIdAsync(taskID)).ReturnsAsync((TaskItem)null);

            var response = await _httpClient.GetAsync($"/api/Task/tasks/{taskID}");
            
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateTask_ReturnsCreatedTask()
        {
            var task = new TaskItem { Title = "Prva naloga", Description = "To je prva naloga.", DueDate = DateTime.Now.AddDays(1), Priority = PriorityLevelEnum.Medium };

            _mockOperations.Reset();

            var response = await _httpClient.PostAsJsonAsync("/api/Task/tasks", task);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdTask = await response.Content.ReadFromJsonAsync<TaskItem>();
            Assert.NotNull(createdTask);
            Assert.Equal(createdTask.Title, task.Title);
            Assert.Equal(createdTask.Description, task.Description);
            Assert.Equal(createdTask.DueDate, task.DueDate);
            Assert.Equal(createdTask.Priority, task.Priority);
        }

        [Fact]
        public async Task CreateTask_InvalidTask()
        {
            var task = new TaskItem();

            _mockOperations.Reset();

            var response = await _httpClient.PostAsJsonAsync("/api/Task/tasks", task);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateTask_ReturnsNoContent()
        {
            var taskID = 1;
            var updatedTask = new TaskItem { ID = 1, Title = "Prva naloga", DueDate = DateTime.Now.AddDays(1), Priority = PriorityLevelEnum.Medium };

            _mockOperations.Reset();
            _mockOperations.Setup(s => s.GetByIdAsync(taskID)).ReturnsAsync(updatedTask);
            _mockOperations.Setup(s => s.UpdateAsync(taskID, updatedTask)).Returns(Task.CompletedTask);

            var response = await _httpClient.PutAsJsonAsync($"/api/Task/tasks/{updatedTask.ID}", updatedTask);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task UpdateTask_ReturnsInvalidTask()
        {
            var taskID = 6;

            _mockOperations.Reset();

            var response = await _httpClient.PutAsJsonAsync($"/api/Task/tasks/{taskID}", (TaskItem)null);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateTask_ReturnsNotFound()
        {
            var updatedTask = new TaskItem { ID = 1, Title = "Prva naloga", DueDate = DateTime.Now.AddDays(1), Priority = PriorityLevelEnum.Medium };

            _mockOperations.Reset();
            _mockOperations.Setup(s => s.GetByIdAsync(updatedTask.ID.Value)).ReturnsAsync((TaskItem)null);

            var response = await _httpClient.PutAsJsonAsync($"/api/Task/tasks/{updatedTask.ID}", updatedTask);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteTask_ReturnsNoContent()
        {
            var updatedTask = new TaskItem { ID = 12, Title = "Prva naloga", DueDate = DateTime.Now.AddDays(1), Priority = PriorityLevelEnum.Medium };

            _mockOperations.Reset();
            _mockOperations.Setup(s => s.GetByIdAsync(updatedTask.ID.Value)).ReturnsAsync(updatedTask);
            _mockOperations.Setup(s => s.DeleteAsync(updatedTask.ID.Value)).Returns(Task.CompletedTask);

            var response = await _httpClient.DeleteAsync($"/api/Task/tasks/{updatedTask.ID}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
