using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrYoklama.Data;   
using QrYoklama.Models.ViewModels;
using QRCoder;



namespace QrYoklama.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.TeacherName = "Ahmet Yılmaz";

            var dersler = new List<string> { "Web Programlama", "Veri Tabanı Yönetimi", "Mobil Uygulama Geliştirme" };

            ViewBag.Derslikler = new List<string> { "Laboratuvar 1", "Laboratuvar 2", "Amfi 3", "Sınıf 102" };
            ViewBag.DersSaatleri = new List<string> { "09:00 - 10:30", "10:45 - 12:15", "13:00 - 14:30", "14:45 - 16:15" };

            return View(dersler);
        }
    }
}