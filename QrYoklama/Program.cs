using QrYoklama.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
builder.Services.AddSignalR();

var azureConnectionString = builder.Configuration["ConnectionStrings:AzureSqlConnection"];

builder.Services.AddDbContext<QrYoklama.Data.QrYoklamaDb>(options =>
    options.UseSqlServer(azureConnectionString));

builder.Services.AddDbContext<QrYoklama.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(azureConnectionString));

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

app.MapHub<AttendanceHub>("/attendanceHub"); 

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<QrYoklama.Data.QrYoklamaDb>();
    var appContext = services.GetRequiredService<QrYoklama.Data.ApplicationDbContext>();
    
    context.Database.EnsureCreated();
    appContext.Database.EnsureCreated();
    
    try
    {
        appContext.Database.ExecuteSqlRaw("DROP TABLE IF EXISTS [Teachers];");
    }
    catch { }
    
    appContext.Database.ExecuteSqlRaw(@"
        CREATE TABLE [Teachers] (
            [Id] INT IDENTITY(1,1) PRIMARY KEY,
            [FirstName] NVARCHAR(MAX) NOT NULL,
            [LastName] NVARCHAR(MAX) NOT NULL,
            [Username] NVARCHAR(MAX) NOT NULL,
            [PasswordHash] NVARCHAR(MAX) NOT NULL,
            [Department] NVARCHAR(MAX) NULL
        )
    ");

    if (!context.Lessons.Any())
    {
        context.Lessons.AddRange(
            new QrYoklama.Models.Lesson { Name = "İnternet Programcılığı", ClassName = "Lab 1" },
            new QrYoklama.Models.Lesson { Name = "Görsel Programlama", ClassName = "Lab 2" },
            new QrYoklama.Models.Lesson { Name = "Yapay Zeka", ClassName = "Lab 3" },
            new QrYoklama.Models.Lesson { Name = "Sunucu İşletim Sistemi", ClassName = "Lab 4" },
            new QrYoklama.Models.Lesson { Name = "Mesleki İngilizce", ClassName = "Lab 5" },
            new QrYoklama.Models.Lesson { Name = "Gömülü Sistemler", ClassName = "Lab 6" },
            new QrYoklama.Models.Lesson { Name = "İçerik Yönetim Sistemi", ClassName = "Lab 1" },
            new QrYoklama.Models.Lesson { Name = "Blokzinciri", ClassName = "Lab 2" }
        );
        context.SaveChanges(); 
    }

    if (!context.Students.Any())
    {
        context.Students.Add(new QrYoklama.Models.Student { Number = "221004001", FullName = "Ahmet Yılmaz" });
        context.Students.Add(new QrYoklama.Models.Student { Number = "221004002", FullName = "Mehmet Demir" });
        context.SaveChanges(); //
    }

    if (!appContext.Teachers.Any())
    {
        appContext.Teachers.AddRange(
            new QrYoklama.Models.Teacher { FirstName = "Yılmaz", LastName = "Koçak", Username = "ykocak", PasswordHash = "123123", Department = "Bilgisayar Programcılığı" },
            new QrYoklama.Models.Teacher { FirstName = "Mehmet", LastName = "Esen", Username = "mehesen", PasswordHash = "112233", Department = "Bilgisayar Programcılığı" },
            new QrYoklama.Models.Teacher { FirstName = "Mesut", LastName = "Özonur", Username = "ozonur", PasswordHash = "123456", Department = "Bilgisayar Programcılığı" },
            new QrYoklama.Models.Teacher { FirstName = "Mehmet İsmail", LastName = "Solmaz", Username = "misolmaz", PasswordHash = "123321", Department = "Bilgisayar Programcılığı" }
        );
        appContext.SaveChanges();
    }
}

app.Run();