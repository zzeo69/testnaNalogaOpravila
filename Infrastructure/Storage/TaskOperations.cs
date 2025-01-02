using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Storage
{
    public class TaskOperations : ITaskOperations
    {
        private readonly InMemoryDbContext _context;

        public TaskOperations(InMemoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _context.TaskItems.ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            return await _context.TaskItems.Where(w => w.ID == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(TaskItem task)
        {
            await _context.TaskItems.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, TaskItem task)
        {
            var existingTask = await _context.TaskItems.Where(w => w.ID == id).FirstOrDefaultAsync();
            if (existingTask == null) return;

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.IsCompleted = task.IsCompleted;
            existingTask.DueDate = task.DueDate;
            existingTask.Priority = task.Priority;
            existingTask.UpdatedDate = task.UpdatedDate;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var task = await _context.TaskItems.Where(w => w.ID == id).FirstOrDefaultAsync();
            if (task != null)
            {
                _context.TaskItems.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
