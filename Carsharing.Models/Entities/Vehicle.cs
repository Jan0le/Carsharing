namespace Carsharing.Models.Entities;

public class Vehicle
{
    public int VehicleId { get; set; }
    public string Model { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; // Available, Booked, Maintenance
    public string CurrentLocation { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

