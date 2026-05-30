using Microsoft.AspNetCore.SignalR;

namespace QrYoklama.Hubs
{
    public class AttendanceHub : Hub
    {
        public async Task JoinLessonRoom(string lessonId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, lessonId);
        }
    }
}
