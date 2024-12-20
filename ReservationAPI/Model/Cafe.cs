namespace ReservationAPI.Model
{
    public class CreateCafe
    {
        public string Name { get; set; } = null!;

        public string Address { get; set; } = null!;

        public TimeOnly OpeningHour { get; set; }

        public TimeOnly ClosingHour { get; set; }

        public int? Capacity { get; set; }

        public bool IsOpen { get; set; }

        public string? Description { get; set; }
    }
}
