using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBookingSystem.Data;
using RoomBookingSystem.Models;
using RoomBookingSystem.ViewModels;

namespace RoomBookingSystem.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;
        public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var today = DateTime.Today;
            var vm = new DashboardViewModel
            {
                TotalRooms = _db.Rooms.Count(),
                PendingRequests = _db.Bookings.Count(b => b.Status == BookingStatus.Pending),
                ApprovedToday = _db.Bookings.Count(b => b.Status == BookingStatus.Approved && b.StartTime.Date == today)
            };

            return View(vm);
        }
    }
}