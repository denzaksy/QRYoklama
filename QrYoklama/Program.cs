
using QrYoklama.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
builder.Services.AddSignalR(); // SignalR servisini ekledik
// QrYoklamaDb context'ini ve Azure SQL ayarlarını sisteme tanıtıyoruz
// Doğrudan appsettings içindeki bölümü açıkça hedef alıyoruz
var azureConnectionString = builder.Configuration["ConnectionStrings:AzureSqlConnection"];

builder.Services.AddDbContext<QrYoklama.Data.QrYoklamaDb>(options =>
    options.UseSqlServer(azureConnectionString));

builder.Services.AddDbContext<QrYoklama.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(azureConnectionString));

// Giriş işlemlerinin (SignInAsync) çalışabilmesi için cookie handler'ı sisteme kaydediyoruz
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapHub<AttendanceHub>("/attendanceHub"); // Hub rotasını bağladık

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<QrYoklama.Data.QrYoklamaDb>();
    var appContext = services.GetRequiredService<QrYoklama.Data.ApplicationDbContext>();
    
    // Veri tabanlarının oluşturulduğundan emin ol
    context.Database.EnsureCreated();
    appContext.Database.EnsureCreated();

    // Eğer hiç ders yoksa örnek ders ekle
    if (!context.Lessons.Any())
    {
        context.Lessons.Add(new QrYoklama.Models.Lesson { Name = "Nesne Yönelimli Programlama", ClassName = "BM-301" });
        context.Lessons.Add(new QrYoklama.Models.Lesson { Name = "Web Tabanlı Teknolojiler", ClassName = "BM-302" });
    }

    // Eğer hiç öğrenci yoksa örnek öğrenci ekle
    if (!context.Students.Any())
    {
        context.Students.Add(new QrYoklama.Models.Student { Number = "221004001", FullName = "Ahmet Yılmaz" });
        context.Students.Add(new QrYoklama.Models.Student { Number = "221004002", FullName = "Mehmet Demir" });
    }

    // Eğer `Teachers` tablosu mevcut değilse oluştur (varsa atla)
    appContext.Database.ExecuteSqlRaw(@"
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Teachers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Teachers](
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [FirstName] NVARCHAR(200) NOT NULL,
        [LastName] NVARCHAR(200) NOT NULL,
        [Username] NVARCHAR(200) NOT NULL,
        [PasswordHash] NVARCHAR(400) NOT NULL,
        [Department] NVARCHAR(200) NULL
    );
END
");

    // Eğer hiç öğretmen yoksa örnek öğretmen ekle (ApplicationDbContext üzerinden)
    if (!appContext.Teachers.Any())
    {
        appContext.Teachers.AddRange(
            new QrYoklama.Models.Teacher { FirstName = "Yılmaz", LastName = "Koçak", Username = "ykocak", PasswordHash = "123123", Department = "Bilgisayar Programcılığı" },
            new QrYoklama.Models.Teacher { FirstName = "Mehmet", LastName = "Esen", Username = "mehesen", PasswordHash = "112233", Department = "Bilgisayar Programcılığı" }
        );
        appContext.SaveChanges();
    }

    context.SaveChanges();
}
app.MapHub<QrYoklama.Hubs.AttendanceHub>("/attendanceHub");
app.Run();