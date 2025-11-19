using Carsharing.Models.Entities;

namespace Carsharing.Controllers.Mvc;

public static class PaymentView
{
    public static void DisplayPayments(List<Payment> payments)
    {
        Console.WriteLine("\n=== Zahlungshistorie ===");
        foreach (var payment in payments)
        {
            Console.WriteLine($"Zahlung #{payment.PaymentId} | {payment.Amount:C} | Status: {payment.Status} | {payment.CreatedAt}");
        }
    }

    public static void ShowPaymentMenu()
    {
        Console.WriteLine("\n=== PaymentService Menu ===");
        Console.WriteLine("1. Zahlungshistorie anzeigen");
        Console.WriteLine("2. Zurück zum Hauptmenü");
    }
}

