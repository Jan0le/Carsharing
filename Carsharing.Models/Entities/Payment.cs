namespace Carsharing.Models.Entities;

public class Payment
{
    public int PaymentId { get; set; }
    public int ParticipantId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty; // Pending, Completed, Failed
    public DateTime CreatedAt { get; set; }
}

