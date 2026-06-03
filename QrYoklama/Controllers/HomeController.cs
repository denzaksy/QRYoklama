using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using QrYoklama.Hubs;
using QRYoklama.Models;

namespace QRYoklama.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHubContext<AttendanceHub> _hubContext; 

    public HomeController(ILogger<HomeController> logger, IHubContext<AttendanceHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetAttendanceFromMobile([FromQuery] string studentNo, [FromQuery] string name)
    {
        if (!string.IsNullOrEmpty(studentNo) && !string.IsNullOrEmpty(name))
        {
            await _hubContext.Clients.All.SendAsync("ReceiveAttendance", studentNo, name);
            return Ok(new { success = true, message = "Sinyal paneli tetiklendi!" });
        }
        
        return BadRequest(new { success = false, message = "Parametreler eksik." });
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}