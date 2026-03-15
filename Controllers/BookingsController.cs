using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomBookingSystem.Services.Interfaces;
using RoomBookingSystem.ViewModels;

namespace RoomBookingSystem.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IAuditService _auditService;

        public BookingsController(IBookingService bookingService, IAuditService auditService)
        {
            _bookingService = bookingService;
            _auditService = auditService;
        }

        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(DateTime start, DateTime end, int? capacity, string? features)
        {
            if (start >= end)
            {
                TempData["ErrorMessage"] = "End time must be later than start time.";
                return RedirectToAction("Search");
            }

            var rooms = await _bookingService.SearchAvailableRoomsAsync(start, end, capacity, features);
            ViewBag.Start = start;
            ViewBag.End = end;
            return View("AvailableRooms", rooms);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid booking data.";
                return RedirectToAction("Search");
            }

            if (model.StartTime >= model.EndTime)
            {
                TempData["ErrorMessage"] = "End time must be later than start time.";
                return RedirectToAction("Search");
            }

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            bool created = await _bookingService.CreateBookingAsync(userId, model);

            if (created)
            {
                await _auditService.LogAsync(
                    userId,
                    "Create Booking",
                    $"User created booking request for RoomID={model.RoomID} from {model.StartTime} to {model.EndTime}"
                );

                TempData["SuccessMessage"] = "Booking request submitted successfully.";
            }
            else
            {
                await _auditService.LogAsync(
                    userId,
                    "Create Booking Failed",
                    $"Booking conflict for RoomID={model.RoomID} from {model.StartTime} to {model.EndTime}"
                );

                TempData["ErrorMessage"] = "Selected room is already booked.";
            }

            return RedirectToAction("Search");
        }

        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Pending()
        {
            var bookings = await _bookingService.GetPendingBookingsAsync();
            return View(bookings);
        }

        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid booking ID.";
                return RedirectToAction("Pending");
            }

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _bookingService.ApproveBookingAsync(id);

            await _auditService.LogAsync(
                userId,
                "Approve Booking",
                $"BookingID={id} approved"
            );

            TempData["SuccessMessage"] = "Booking approved successfully.";
            return RedirectToAction("Pending");
        }

        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Reject(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid booking ID.";
                return RedirectToAction("Pending");
            }

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _bookingService.RejectBookingAsync(id);

            await _auditService.LogAsync(
                userId,
                "Reject Booking",
                $"BookingID={id} rejected"
            );

            TempData["SuccessMessage"] = "Booking rejected successfully.";
            return RedirectToAction("Pending");
        }
    }
}