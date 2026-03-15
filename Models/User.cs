using System.ComponentModel.DataAnnotations;

namespace RoomBookingSystem.Models
{
    public enum UserRole
    {
        Employee,
        Manager,
        Admin
    }

    public class User
    {
        public int UserID { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
    }
}