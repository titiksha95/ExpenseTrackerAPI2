using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTrackerAPI2.Models
{
    [Table("ExpenseCategories")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        public int UserId { get; set; }

        public User? User { get; set; }

        public ICollection<ExpenseTransaction> Transactions { get; set; }
            = new List<ExpenseTransaction>();
    }
}