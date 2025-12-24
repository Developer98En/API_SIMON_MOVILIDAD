namespace Api.Models
{
    public class Alert
    {
        public long Id { get; set; }

        public Guid DeviceId { get; set; }
        public Device Device { get; set; } = null!;

        public string Message { get; set; } = "";
        public int PredictedMinutesLeft { get; set; }

        public bool Resolved { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
