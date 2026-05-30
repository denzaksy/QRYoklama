using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Threading.Tasks;

namespace QrYoklama.Controllers
{
    public class GenerateQrRequest
    {
        public string Payload { get; set; } = string.Empty;
    }

    [ApiController]
    [Route("api/[controller]")]
    public class QrController : ControllerBase
    {
        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] GenerateQrRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.Payload))
                return BadRequest(new { message = "Payload boş olamaz." });

            // QRCoder ile PNG byte üretimi
            using var qrGenerator = new QRCodeGenerator();
            using var qrData = qrGenerator.CreateQrCode(model.Payload, QRCodeGenerator.ECCLevel.Q);
            var png = new PngByteQRCode(qrData).GetGraphic(20);

            return File(png, "image/png");
        }
    }
}
