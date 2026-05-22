namespace QrYoklama.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty; 
        public string PasswordHash { get; set; } = string.Empty; 
        public string Department { get; set; } = string.Empty;
    }
}