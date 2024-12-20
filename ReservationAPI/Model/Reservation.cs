namespace ReservationAPI.Model
{
    public class CreateReservation
    {
        public int UsersId { get; set; }
        public string Username { get; set; } = null!;

        public int CafeId { get; set; } 

        public DateOnly ReservationDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public int NumberOfGuests { get; set; } 

        public string? Notes { get; set; }
    }
}
