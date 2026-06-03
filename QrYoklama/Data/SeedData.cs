using Microsoft.EntityFrameworkCore;
using QrYoklama.Models;
using System;
using System.Linq;

namespace QrYoklama.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                context.Database.EnsureCreated();

                if (context.Teachers.Any())
                {
                    context.Teachers.RemoveRange(context.Teachers);
                    context.SaveChanges();
                }

                context.Teachers.Add(new Teacher
                {
                    FirstName = "Yılmaz",
                    LastName = "Koçak",
                    Username = "ykocak",
                    PasswordHash = "123123", 
                    Department = "Bilgisayar Programcılığı"
                });

                context.Teachers.Add(new Teacher
                {
                    FirstName = "Mehmet",
                    LastName = "Esen",
                    Username = "mehesen",
                    PasswordHash = "112233", 
                    Department = "Bilgisayar Programcılığı"
                });

                context.Teachers.Add(new Teacher
                {
                    FirstName = "Mesut",
                    LastName = "Özonur",
                    Username = "ozonur",
                    PasswordHash = "123456", 
                    Department = "Bilgisayar Programcılığı"
                });

                context.Teachers.Add(new Teacher
                {
                    FirstName = "Mehmet İsmail",
                    LastName = "Solmaz",
                    Username = "misolmaz",
                    PasswordHash = "123321", 
                    Department = "Bilgisayar Programcılığı"
                });

                context.SaveChanges();
            }
        }
    }
}