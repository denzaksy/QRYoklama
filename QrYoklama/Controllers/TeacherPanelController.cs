using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QrYoklama.Models.ViewModels;
using System.Linq;
using System.Security.Claims;

namespace QrYoklama.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherPanelController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.TeacherName = User.Identity?.Name;
            var teacherUsername = User.FindFirst(ClaimTypes.UserData)?.Value ?? string.Empty;

            var viewModel = new TeacherPanelIndexViewModel
            {
                Courses = TeacherPanelSchedule.GetCoursesForTeacher(teacherUsername).ToList(),
                Rooms = TeacherPanelSchedule.Rooms.ToList(),
                TimeSlots = TeacherPanelSchedule.TimeSlots.ToList()
            };

            return View(viewModel);
        }
    }
}