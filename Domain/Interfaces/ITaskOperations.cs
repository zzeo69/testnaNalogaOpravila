using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITaskOperations
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(int id);
        Task AddAsync(TaskItem item);
        Task UpdateAsync(int id, TaskItem item);
        Task DeleteAsync(int id);
    }
}
