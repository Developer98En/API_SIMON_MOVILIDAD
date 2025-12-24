namespace Api.Models
{
    public class UserDevice
    {

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;


        public Guid DeviceId { get; set; }
        public Device Device { get; set; } = null!;
    }
}
