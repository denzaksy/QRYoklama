using Microsoft.AspNetCore.Mvc;
using QrYoklama.Models.ViewModels;

namespace QrYoklama.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Scan(string lessonName, string room, string time, string token)
        {
            var model = new StudentScanViewModel
            {
                LessonName = lessonName ?? string.Empty,
                Room = room ?? string.Empty,
                Time = time ?? string.Empty,
                Token = token ?? string.Empty
            };

            return View(model);
        }
    }
}
