using System;

namespace QrYoklama.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }

    public class Lesson
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
    }

    public class AttendanceRecord
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public DateTime Date { get; set; }
        public DateTime ScanTime { get; set; }
        public string DeviceInfo { get; set; } = string.Empty;
    }
}
