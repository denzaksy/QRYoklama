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
            // 1. Yukarıdaki Hoş geldiniz kısmında hocanın adı yazsın diye:
            ViewBag.TeacherName = "Ahmet Yılmaz"; // Burayı istersen dinamik (User.Identity.Name) yapabilirsin

            // 2. Derslerin listesi (Hocanın seçebileceği model)
            var dersler = new List<string> { "Web Programlama", "Veri Tabanı Yönetimi", "Mobil Uygulama Geliştirme" };

            // 3. Derslikler ve Saatler (ViewBag ile gönderdiklerimiz)
            ViewBag.Derslikler = new List<string> { "Laboratuvar 1", "Laboratuvar 2", "Amfi 3", "Sınıf 102" };
            ViewBag.DersSaatleri = new List<string> { "09:00 - 10:30", "10:45 - 12:15", "13:00 - 14:30", "14:45 - 16:15" };

            // Modeli (ders listesini) View'a fırlatıyoruz
            return View(dersler);
        }
    }
}