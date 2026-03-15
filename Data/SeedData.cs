using RoomBookingSystem.Models;
using RoomBookingSystem.Services.Interfaces;

namespace RoomBookingSystem.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context, IAuthService authService)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        FullName = "Admin User",
                        Email = "admin@test.com",
                        PasswordHash = authService.HashPassword("123456"),
                        Role = UserRole.Admin
                    },
                    new User
                    {
                        FullName = "Manager User",
                        Email = "manager@test.com",
                        PasswordHash = authService.HashPassword("123456"),
                        Role = UserRole.Manager
                    },
                    new User
                    {
                        FullName = "Employee User",
                        Email = "employee@test.com",
                        PasswordHash = authService.HashPassword("123456"),
                        Role = UserRole.Employee
                    }
                );
            }

            if (!context.Rooms.Any())
            {
                context.Rooms.AddRange(
                    new Room
                    {
                        RoomName = "Conference Room A",
                        Capacity = 20,
                        Location = "1st Floor",
                        Features = "Projector, WiFi, AC",
                        Status = RoomStatus.Available
                    },
                    new Room
                    {
                        RoomName = "Meeting Room B",
                        Capacity = 10,
                        Location = "2nd Floor",
                        Features = "TV, Whiteboard, WiFi",
                        Status = RoomStatus.Available
                    }
                );
            }

            context.SaveChanges();
        }
    }
}