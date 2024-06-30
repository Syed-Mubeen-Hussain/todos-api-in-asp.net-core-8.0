using System.ComponentModel.DataAnnotations;

namespace TodosApi.Models
{
    public class Todo
    {
        public int Id { get; set; }

        [StringLength(125)]
        public string Title { get; set; }

        [StringLength(5000)]
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }

        [StringLength(50)]
        public string Priority { get; set; }
    }
}
