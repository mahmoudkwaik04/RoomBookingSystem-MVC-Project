using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomBookingSystem.Models;
using RoomBookingSystem.Services.Interfaces;

namespace RoomBookingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoomsController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IAuditService _auditService;

        public RoomsController(IRoomService roomService, IAuditService auditService)
        {
            _roomService = roomService;
            _auditService = auditService;
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return View(rooms);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room room)
        {
            if (!ModelState.IsValid)
                return View(room);

            await _roomService.CreateRoomAsync(room);

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _auditService.LogAsync(
                userId,
                "Add Room",
                $"Added room: {room.RoomName}, Capacity={room.Capacity}, Location={room.Location}, Status={room.Status}"
            );

            TempData["SuccessMessage"] = "Room added successfully.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
                return NotFound();

            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Room room)
        {
            if (!ModelState.IsValid)
                return View(room);

            await _roomService.UpdateRoomAsync(room);

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _auditService.LogAsync(
                userId,
                "Edit Room",
                $"Edited room: RoomID={room.RoomID}, Name={room.RoomName}, Capacity={room.Capacity}, Location={room.Location}, Status={room.Status}"
            );

            TempData["SuccessMessage"] = "Room updated successfully.";
            return RedirectToAction("Index");
        }
    }
}