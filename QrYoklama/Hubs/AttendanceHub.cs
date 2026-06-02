using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace QrYoklama.Hubs
{
    public class AttendanceHub : Hub
    {
        public async Task SendAttendance(string studentNumber, string studentName)
        {
            await Clients.All.SendAsync("ReceiveAttendance", studentNumber, studentName);
        }
    }
}