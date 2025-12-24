namespace Api.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";

        public string Role { get; set; } = "USER";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserDevice> UserDevices { get; set; } = new List<UserDevice>();
    }
}
