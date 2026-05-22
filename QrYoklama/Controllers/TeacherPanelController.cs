using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QRYoklamaSistemi.Controllers
{
    [Authorize] // Sadece giriş yapmış kullanıcılar girebilir
    public class TeacherPanelController : Controller
    {
        public IActionResult Index()
        {
            // Giriş yapan hocanın bilgilerini çekebiliriz
            var teacherEmail = User.Identity?.Name; 
            return View();
        }
    }
}