using System.Collections.Generic;

namespace QrYoklama.Models.ViewModels
{
    public class TeacherAttendanceViewModel
    {
        public int LessonId { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public List<AttendedStudentDto> AttendedStudents { get; set; } = new();
    }

    public class AttendedStudentDto
    {
        public string Number { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string ScanTime { get; set; } = string.Empty;
    }
}
