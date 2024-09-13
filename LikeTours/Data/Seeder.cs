using LikeTours.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace LikeTours.Data
{
    public static class Seeder
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (!context.Users.Any(u => u.Email == "admin@likeTours.com"))
                {
                    var adminUser = new User
                    {
                        Username = "admin",
                        Email = "admin@likeTours.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("Password")
                    };

                    context.Users.Add(adminUser);
                    context.SaveChanges();
                }
            }
        }
    }
}
