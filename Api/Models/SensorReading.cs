namespace Api.Models
{
    public class SensorReading
    {
        public long Id { get; set; }

        public Guid DeviceId { get; set; }
        public Device Device { get; set; } = null!;

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Speed { get; set; }
        public double FuelLevel { get; set; }
        public double Temperature { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
