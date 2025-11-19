using Carsharing.Models.Entities;

namespace Carsharing.Controllers.Mvc;

public static class BookingView
{
    public static (int VehicleId, int ParticipantId, DateTime StartTime, DateTime EndTime) GetBookingDetails()
    {
        Console.WriteLine("\n=== Neue Buchung ===");
        Console.Write("Fahrzeug ID: ");
        int vehicleId = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Teilnehmer ID: ");
        int participantId = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Startzeit (dd.MM.yyyy HH:mm): ");
        DateTime startTime = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.ToString());
        Console.Write("Endzeit (dd.MM.yyyy HH:mm): ");
        DateTime endTime = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.ToString());

        return (vehicleId, participantId, startTime, endTime);
    }

    public static void DisplayBookings(List<Booking> bookings)
    {
        Console.WriteLine("\n=== Meine Buchungen ===");
        foreach (var booking in bookings)
        {
            Console.WriteLine($"Buchung #{booking.BookingId} | Fahrzeug: {booking.VehicleId} | " +
                            $"Status: {booking.BookingStatus} | {booking.StartTime} - {booking.EndTime}");
        }
    }

    public static void ShowBookingMenu()
    {
        Console.WriteLine("\n=== BookingService Menu ===");
        Console.WriteLine("1. Neue Buchung erstellen");
        Console.WriteLine("2. Meine Buchungen anzeigen");
        Console.WriteLine("3. Zurück zum Hauptmenü");
    }
}

