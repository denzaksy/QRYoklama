using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QrYoklama.Models.ViewModels;
using System.Linq;

namespace QrYoklama.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherPanelController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.TeacherName = User.Identity?.Name;

            var viewModel = new TeacherPanelIndexViewModel
            {
                Courses = TeacherPanelSchedule.Courses.ToList(),
                Rooms = TeacherPanelSchedule.Rooms.ToList(),
                TimeSlots = TeacherPanelSchedule.TimeSlots.ToList()
            };

            return View(viewModel);
        }
    }
}