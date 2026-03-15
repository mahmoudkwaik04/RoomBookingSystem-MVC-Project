using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomBookingSystem.Models
{
    public enum BookingStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class Booking
    {
        public int BookingID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public User? User { get; set; }

        [ForeignKey("Room")]
        public int RoomID { get; set; }
        public Room? Room { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Pending;
    }
}