using Microsoft.EntityFrameworkCore;
using RoomBookingSystem.Data;
using RoomBookingSystem.Models;
using RoomBookingSystem.Services.Interfaces;
using RoomBookingSystem.ViewModels;

namespace RoomBookingSystem.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Room>> SearchAvailableRoomsAsync(DateTime start, DateTime end, int? capacity, string? features)
        {
            var rooms = _context.Rooms.Where(r => r.Status == RoomStatus.Available);

            if (capacity.HasValue)
                rooms = rooms.Where(r => r.Capacity >= capacity.Value);

            if (!string.IsNullOrWhiteSpace(features))
                rooms = rooms.Where(r => r.Features.Contains(features));

            var bookedRoomIds = await _context.Bookings
                .Where(b => b.Status == BookingStatus.Approved &&
                            start < b.EndTime &&
                            end > b.StartTime)
                .Select(b => b.RoomID)
                .ToListAsync();

            return await rooms
                .Where(r => !bookedRoomIds.Contains(r.RoomID))
                .ToListAsync();
        }

        public async Task<bool> CreateBookingAsync(int userId, BookingCreateViewModel model)
        {
            bool conflict = await _context.Bookings.AnyAsync(b =>
                b.RoomID == model.RoomID &&
                b.Status == BookingStatus.Approved &&
                model.StartTime < b.EndTime &&
                model.EndTime > b.StartTime);

            if (conflict) return false;

            var booking = new Booking
            {
                UserID = userId,
                RoomID = model.RoomID,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Status = BookingStatus.Pending
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Booking>> GetPendingBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .Where(b => b.Status == BookingStatus.Pending)
                .ToListAsync();
        }

        public async Task ApproveBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null)
            {
                booking.Status = BookingStatus.Approved;
                await _context.SaveChangesAsync();
            }
        }

        public async Task RejectBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null)
            {
                booking.Status = BookingStatus.Rejected;
                await _context.SaveChangesAsync();
            }
        }
    }
}