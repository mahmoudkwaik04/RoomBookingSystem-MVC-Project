using RoomBookingSystem.Data;
using RoomBookingSystem.Models;
using RoomBookingSystem.Services.Interfaces;

namespace RoomBookingSystem.Services
{
    public class AuditService : IAuditService
    {
        private readonly ApplicationDbContext _context;

        public AuditService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(int userId, string action, string details)
        {
            _context.AuditLogs.Add(new AuditLog
            {
                UserID = userId,
                Action = action,
                Details = details,
                Timestamp = DateTime.Now
            });

            await _context.SaveChangesAsync();
        }
    }
}