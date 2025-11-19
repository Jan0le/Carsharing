using Carsharing.Models.Entities;
using Carsharing.Services.Interfaces;

namespace Carsharing.Services.Implementations;

public class PaymentService : IPaymentService
{
    private List<Payment> _payments = new();
    private int _nextId = 1;

    public bool ProcessPayment(int participantId, decimal amount, bool confirmPayment = true)
    {
        // Simuliere Zahlungsprozess
        // In Blazor wird die Bestätigung über den confirmPayment Parameter gesteuert
        bool success = confirmPayment;

        var payment = new Payment
        {
            PaymentId = _nextId++,
            ParticipantId = participantId,
            Amount = amount,
            Status = success ? "Completed" : "Failed",
            CreatedAt = DateTime.Now
        };

        _payments.Add(payment);

        return success;
    }

    public List<Payment> GetPaymentHistory(int participantId)
    {
        return _payments.Where(p => p.ParticipantId == participantId).ToList();
    }
}

