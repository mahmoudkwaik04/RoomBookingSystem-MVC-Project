using System.ComponentModel.DataAnnotations;

namespace RoomBookingSystem.Models
{
    public class Report
    {
        [Key]
        public int ReportID { get; set; }

        public string ReportType { get; set; } = string.Empty;
        public DateTime GeneratedDate { get; set; } = DateTime.Now;
        public string Content { get; set; } = string.Empty;
    }
}