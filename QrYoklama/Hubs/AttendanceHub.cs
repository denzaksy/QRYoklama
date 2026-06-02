using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace QrYoklama.Hubs // Klasör ve ad alanını buraya kesin olarak dikiyoruz
{
    public class AttendanceHub : Hub
    {
        public async Task SendAttendance(string studentNumber, string studentName)
        {
            await Clients.All.SendAsync("ReceiveAttendance", studentNumber, studentName);
        }
    }
}