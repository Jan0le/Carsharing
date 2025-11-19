using Carsharing.Models.Entities;
using Carsharing.Services.Interfaces;

namespace Carsharing.Services.Implementations;

public class PaymentService : IPaymentService
{
    private List<Payment> _payments = new();
    private int _nextId = 1;

    public bool ProcessPayment(int participantId, decimal amount)
    {
        Console.WriteLine($"Zahlungsanfrage für Teilnehmer {participantId}: {amount:C}");

        // Simuliere Zahlungsprozess
        Console.Write("Zahlung bestätigen? (j/n): ");
        string? response = Console.ReadLine()?.ToLower();
        bool success = response == "j";

        var payment = new Payment
        {
            PaymentId = _nextId++,
            ParticipantId = participantId,
            Amount = amount,
            Status = success ? "Completed" : "Failed",
            CreatedAt = DateTime.Now
        };

        _payments.Add(payment);

        if (success)
        {
            Console.WriteLine("Zahlung erfolgreich!");
        }
        else
        {
            Console.WriteLine("Zahlung fehlgeschlagen!");
        }

        return success;
    }

    public List<Payment> GetPaymentHistory(int participantId)
    {
        return _payments.Where(p => p.ParticipantId == participantId).ToList();
    }
}

