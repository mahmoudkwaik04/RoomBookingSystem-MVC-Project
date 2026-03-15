using System.ComponentModel.DataAnnotations;

namespace RoomBookingSystem.ViewModels
{
    public class BookingCreateViewModel
    {
        [Required]
        public int RoomID { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
    }
}