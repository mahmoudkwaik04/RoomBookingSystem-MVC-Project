using System.ComponentModel.DataAnnotations;

namespace RoomBookingSystem.Models
{
    public enum RoomStatus
    {
        Available,
        Occupied,
        Maintenance
    }

    public class Room
    {
        public int RoomID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Room Name")]
        public string RoomName { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000)]
        public int Capacity { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        [Display(Name = "Features / Amenities")]
        public string Features { get; set; } = string.Empty;

        [Required]
        public RoomStatus Status { get; set; } = RoomStatus.Available;

        public ICollection<Booking>? Bookings { get; set; }
    }
}