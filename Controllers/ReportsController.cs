using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBookingSystem.Data;

namespace RoomBookingSystem.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalBookings = await _context.Bookings.CountAsync();
            var approved = await _context.Bookings.CountAsync(b => b.Status == Models.BookingStatus.Approved);
            var pending = await _context.Bookings.CountAsync(b => b.Status == Models.BookingStatus.Pending);
            var rejected = await _context.Bookings.CountAsync(b => b.Status == Models.BookingStatus.Rejected);

            ViewBag.TotalBookings = totalBookings;
            ViewBag.Approved = approved;
            ViewBag.Pending = pending;
            ViewBag.Rejected = rejected;

            return View();
        }
    }
}