namespace Api.Models
{
    public class Device
    {
        public Guid Id { get; set; }

        public string RealDeviceId { get; set; } = "";
        public string MaskedDeviceId { get; set; } = "";
        public string VehiclePlate { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserDevice> UserDevices { get; set; } = new List<UserDevice>();
        public ICollection<SensorReading> SensorReadings { get; set; } = new List<SensorReading>();
        public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
    }
}
