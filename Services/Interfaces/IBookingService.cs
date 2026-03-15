using RoomBookingSystem.Models;
using RoomBookingSystem.ViewModels;

namespace RoomBookingSystem.Services.Interfaces
{
    public interface IBookingService
    {
        Task<List<Room>> SearchAvailableRoomsAsync(DateTime start, DateTime end, int? capacity, string? features);
        Task<bool> CreateBookingAsync(int userId, BookingCreateViewModel model);
        Task<List<Booking>> GetPendingBookingsAsync();
        Task ApproveBookingAsync(int bookingId);
        Task RejectBookingAsync(int bookingId);
    }
}