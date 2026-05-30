using Microsoft.EntityFrameworkCore;
using QrYoklama.Models;

namespace QrYoklama.Data
{
    public class QrYoklamaDb : DbContext
    {
        public QrYoklamaDb(DbContextOptions<QrYoklamaDb> options) : base(options)
        {
        }

        // Bu satırlar Azure üzerinde oluşturulacak tabloları temsil eder
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Lesson> Lessons { get; set; } = null!;
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; } = null!;

        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Yoklama tablosu ile Öğrenci tablosu arasındaki ilişki
            modelBuilder.Entity<AttendanceRecord>()
                .HasOne(a => a.Student)
                .WithMany()
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Yoklama tablosu ile Ders tablosu arasındaki ilişki
            modelBuilder.Entity<AttendanceRecord>()
                .HasOne(a => a.Lesson)
                .WithMany()
                .HasForeignKey(a => a.LessonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}