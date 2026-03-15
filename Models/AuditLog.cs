using System.ComponentModel.DataAnnotations;

namespace RoomBookingSystem.Models
{
    public class AuditLog
    {
        [Key]
        public int AuditLogID { get; set; }

        public int UserID { get; set; }

        [Required]
        public string Action { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.Now;

        public string Details { get; set; } = string.Empty;
    }
}