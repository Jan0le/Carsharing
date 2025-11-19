namespace Carsharing.Models.Entities;

public class Booking
{
    public int BookingId { get; set; }
    public int VehicleId { get; set; }
    public int ParticipantId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string BookingStatus { get; set; } = string.Empty; // Pending, Confirmed, Cancelled
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

