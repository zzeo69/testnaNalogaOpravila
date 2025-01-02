using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class TaskItem
    {
        public int? ID { get; set; }

        [Required]
        public string? Title { get; set; }

        public string? Description { get; set; } = string.Empty;

        public bool? IsCompleted { get; set; } = false;

        [Required]
        public DateTime? DueDate { get; set; }

        [Required]
        public PriorityLevelEnum? Priority { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
