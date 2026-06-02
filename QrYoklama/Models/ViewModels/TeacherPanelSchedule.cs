using System.Linq;

namespace QrYoklama.Models.ViewModels
{
    public class TeacherPanelIndexViewModel
    {
        public List<string> Courses { get; set; } = new();
        public List<string> Rooms { get; set; } = new();
        public List<string> TimeSlots { get; set; } = new();
    }

    public class PresetScheduleItem
    {
        public string Course { get; set; } = string.Empty;
        public string Room { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
    }

    public static class TeacherPanelSchedule
    {
        public static IReadOnlyList<string> Courses { get; } = new List<string>
        {
            "İnternet Programcılığı",
            "İçerik Yönetim Sistemi",
            "Görsel Programlama",
            "Sunucu İşletim Sistemi",
            "Mesleki İngilizce"
        };

        public static IReadOnlyList<string> Rooms { get; } = new List<string>
        {
            "Lab 1",
            "Lab 2",
            "Lab 3",
            "Lab 4",
            "Lab 5",
            "Lab 6"
        };

        public static IReadOnlyList<string> TimeSlots { get; } = new List<string>
        {
            "08:15 - 10:00",
            "10:15 - 12:00",
            "13:15 - 15:00",
            "15:15 - 17:00"
        };

        public static IEnumerable<PresetScheduleItem> Presets =>
            Courses.SelectMany(course => TimeSlots, (course, time) => new PresetScheduleItem
            {
                Course = course,
                Time = time,
                Room = Rooms.FirstOrDefault() ?? string.Empty
            });
    }
}
