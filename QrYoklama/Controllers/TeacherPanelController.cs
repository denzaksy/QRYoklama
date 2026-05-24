using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace QrYoklama.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherPanelController : Controller
    {
        public IActionResult Index()
        {
            var teacherName = User.Identity?.Name;
            ViewBag.TeacherName = teacherName;

            var dersler = new List<string> 
            { 
                "İnternet Programcılığı", 
                "İçerik Yönetim Sistemi", 
                "Görsel Programlama", 
                "Sunucu İşletim Sistemi",
                "Mesleki İngilizce" 
            };

            ViewBag.Derslikler = new List<string>
            {
                "Lab 1",
                "Lab 2",
                "Lab 3",
                "Lab 4",
                "Lab 5",
                "Lab 6"
            };

            ViewBag.DersSaatleri = new List<string>
            {
                "08:15 - 10:00",
                "10:15 - 12:00",
                "13:15 - 15:00",
                "15:15 - 17:00"
            };

            return View(dersler);
        }
    }
}