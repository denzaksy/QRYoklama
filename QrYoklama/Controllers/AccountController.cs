using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using QrYoklama.Data; 
using QrYoklama.Models; 
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace QrYoklama.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Kullanıcı adı ve şifre alanı boş bırakılamaz.";
                return View();
            }

            if (!_context.Teachers.Any())
            {
                var varsayilanHocalar = new List<Teacher>
                {
                    new Teacher { FirstName = "Yılmaz", LastName = "Koçak", Username = "ykocak", PasswordHash = "123123", Department = "Bilgisayar Programcılığı" },
                    new Teacher { FirstName = "Mehmet", LastName = "Esen", Username = "mehesen", PasswordHash = "112233", Department = "Bilgisayar Programcılığı" },
                    new Teacher { FirstName = "Mesut", LastName = "Özonur", Username = "ozonur", PasswordHash = "123456", Department = "Bilgisayar Programcılığı" },
                    new Teacher { FirstName = "Mehmet İsmail", LastName = "Solmaz", Username = "misolmaz", PasswordHash = "123321", Department = "Bilgisayar Programcılığı" }
                };

                _context.Teachers.AddRange(varsayilanHocalar);
                _context.SaveChanges();
            }

            var teacher = _context.Teachers
                .FirstOrDefault(t => t.Username == username && t.PasswordHash == password);

            if (teacher != null) 
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, $"{teacher.FirstName} {teacher.LastName}"),
                    new Claim(ClaimTypes.UserData, teacher.Username),
                    new Claim(ClaimTypes.Role, "Teacher")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = System.DateTimeOffset.UtcNow.AddHours(2)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, 
                    new ClaimsPrincipal(claimsIdentity), 
                    authProperties
                );

                return RedirectToAction("Index", "TeacherPanel");
            }

            ViewBag.Error = "Geçersiz kullanıcı adı veya şifre!";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}