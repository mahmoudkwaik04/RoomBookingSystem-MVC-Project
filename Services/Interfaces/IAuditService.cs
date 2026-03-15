using RoomBookingSystem.Models;

namespace RoomBookingSystem.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User?> LoginAsync(string email, string password);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
namespace RoomBookingSystem.Services.Interfaces
{
    public interface IAuditService
    {
        Task LogAsync(int userId, string action, string details);
    }
}