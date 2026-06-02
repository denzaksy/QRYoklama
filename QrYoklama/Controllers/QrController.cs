using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using QRCoder;
using QrYoklama.Models.ViewModels;
using System;
using System.Linq;

namespace QrYoklama.Controllers
{
    public class GenerateQrRequest
    {
        public string Payload { get; set; } = string.Empty;
    }

    public class PresetQrCodeDto
    {
        public string Course { get; set; } = string.Empty;
        public string Room { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public string QrBase64 { get; set; } = string.Empty;
    }

    [ApiController]
    [Route("api/[controller]")]
    public class QrController : ControllerBase
    {
        private readonly IMemoryCache _cache;

        public QrController(IMemoryCache cache)
        {
            _cache = cache;
        }

        [HttpPost("generate")]
        public IActionResult Generate([FromBody] GenerateQrRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.Payload))
                return BadRequest(new { message = "Payload boş olamaz." });

            var png = GenerateQrPng(model.Payload);
            return File(png, "image/png");
        }

        [HttpGet("presets")]
        public IActionResult GetPresetQrs()
        {
            var presetQrCodes = TeacherPanelSchedule.Presets
                .Select(schedule => new PresetQrCodeDto
                {
                    Course = schedule.Course,
                    Time = schedule.Time,
                    Room = schedule.Room,
                    QrBase64 = GetCachedQrBase64(schedule)
                })
                .ToList();

            return Ok(presetQrCodes);
        }

        private string GetCachedQrBase64(PresetScheduleItem schedule)
        {
            var cacheKey = GetCacheKey(schedule);
            return _cache.GetOrCreate<string>(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4);
                var scanUrl = BuildScanUrl(schedule.Course, schedule.Room, schedule.Time, cacheKey);
                var png = GenerateQrPng(scanUrl);
                return Convert.ToBase64String(png);
            });
        }

        private static string GetCacheKey(PresetScheduleItem schedule)
        {
            return $"presetqr:{schedule.Course}|{schedule.Room}|{schedule.Time}";
        }

        private string BuildScanUrl(string course, string room, string time, string token)
        {
            var encodedCourse = Uri.EscapeDataString(course);
            var encodedRoom = Uri.EscapeDataString(room ?? string.Empty);
            var encodedTime = Uri.EscapeDataString(time);
            var encodedToken = Uri.EscapeDataString(token);

            var scheme = Request.Scheme ?? "https";
            var host = Request.Host.ToUriComponent();
            return $"{scheme}://{host}/Student/Scan?lessonName={encodedCourse}&room={encodedRoom}&time={encodedTime}&token={encodedToken}";
        }

        private static byte[] GenerateQrPng(string payload)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            return new PngByteQRCode(qrData).GetGraphic(20);
        }
    }
}
