using System;
using System.Collections.Generic;

namespace ReservationAPI.Database;

public partial class Reservation
{
    public int Id { get; set; }

    public int UsersId { get; set; }

    public string Username { get; set; } = null!;

    public int CafeId { get; set; }

    public DateOnly ReservationDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public int NumberOfGuests { get; set; }

    public string? Notes { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
