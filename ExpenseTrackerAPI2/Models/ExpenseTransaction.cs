using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTrackerAPI2.Models
{
    [Table("ExpenseTransactions")]
    public class ExpenseTransaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(20)]
        public string Type { get; set; } = string.Empty;

        public DateTime TransactionDate { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        // Category Foreign Key
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        // User Foreign Key
        public int UserId { get; set; }

        public User? User { get; set; }
    }
}