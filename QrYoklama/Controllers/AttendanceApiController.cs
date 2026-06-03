using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QrYoklama.Controllers
{    public class QrScanRequestDto
    {
        public int? LessonId { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public string Room { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public string StudentNumber { get; set; } = string.Empty;
        public string? DeviceInfo { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceApiController : ControllerBase
    {
        private readonly QrYoklama.Data.QrYoklamaDb _context;
        private readonly IHubContext<QrYoklama.Hubs.AttendanceHub> _hubContext;

        public AttendanceApiController(QrYoklama.Data.QrYoklamaDb context, IHubContext<QrYoklama.Hubs.AttendanceHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPost("scan")]
        public async Task<IActionResult> ProcessStudentQr([FromBody] QrScanRequestDto model)
        {
            Console.WriteLine($"==================================================");
            Console.WriteLine($"👉 QR ISTEGI GELDI! Öğrenci: {model.StudentNumber} | Ders: {model.LessonName} ({model.LessonId})");
            Console.WriteLine($"==================================================");

            if (string.IsNullOrWhiteSpace(model.StudentNumber))
            {
                return BadRequest(new { message = "Öğrenci numarası boş bırakılamaz." });
            }

            var student = await _context.Students.FirstOrDefaultAsync(s => s.Number == model.StudentNumber);
            if (student == null)
            {
                return BadRequest(new { message = "Öğrenci bulunamadı!" });
            }

            QrYoklama.Models.Lesson? lesson = null;
            if (model.LessonId.HasValue && model.LessonId.Value > 0)
            {
                lesson = await _context.Lessons.FindAsync(model.LessonId.Value);
            }

            if (lesson == null && !string.IsNullOrWhiteSpace(model.LessonName))
            {
                lesson = await _context.Lessons.FirstOrDefaultAsync(l => l.Name == model.LessonName);
                if (lesson == null)
                {
                    lesson = new QrYoklama.Models.Lesson
                    {
                        Name = model.LessonName,
                        ClassName = string.IsNullOrWhiteSpace(model.Room) ? model.LessonName : model.Room
                    };
                    _context.Lessons.Add(lesson);
                    await _context.SaveChangesAsync();
                }
            }

            if (lesson == null)
            {
                return BadRequest(new { message = "Ders bilgisi bulunamadı." });
            }

            var alreadyAttended = await _context.AttendanceRecords
                .AnyAsync(a => a.LessonId == lesson.Id && a.StudentId == student.Id && a.Date.Date == DateTime.Today);

            if (alreadyAttended)
            {
                return BadRequest(new { message = "Bu ders için zaten yoklama verdiniz." });
            }

            var record = new QrYoklama.Models.AttendanceRecord
            {
                LessonId = lesson.Id,
                StudentId = student.Id,
                ScanTime = DateTime.Now,
                Date = DateTime.Today,
                DeviceInfo = model.DeviceInfo ?? "Mobil Uygulama"
            };

            _context.AttendanceRecords.Add(record);
            await _context.SaveChangesAsync();

            var groupName = lesson.Name;
            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveAttendance", new
            {
                studentNumber = student.Number,
                studentName = student.FullName,
                scanTime = record.ScanTime.ToString("HH:mm:ss")
            });

            Console.WriteLine($"✅ {student.FullName} için yoklama yazıldı. SignalR tetiklendi.");
            return Ok(new
            {
                success = true,
                message = "Yoklamanız başarıyla alındı!",
                student = new
                {
                    number = student.Number,
                    name = student.FullName
                },
                lesson = new
                {
                    name = lesson.Name,
                    room = lesson.ClassName,
                    time = model.Time
                },
                scanTime = record.ScanTime.ToString("HH:mm:ss"),
                date = record.Date.ToString("dd.MM.yyyy")
            });
        }
    }
}