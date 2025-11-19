using Carsharing.Models.Entities;
using Carsharing.Services.Interfaces;

namespace Carsharing.Services.Implementations;

public class BookingService : IBookingService
{
    private List<Booking> _bookings = new();
    private int _nextId = 1;

    // Integration mit anderen Services
    private readonly IVehicleService _vehicleService;
    private readonly IParticipantService _participantService;
    private readonly IPaymentService _paymentService;

    public BookingService(IVehicleService vehicleService, IParticipantService participantService, IPaymentService paymentService)
    {
        _vehicleService = vehicleService;
        _participantService = participantService;
        _paymentService = paymentService;
    }

    public bool CreateBooking(int vehicleId, int participantId, DateTime startTime, DateTime endTime)
    {
        // Prüfe Fahrzeugverfügbarkeit
        var vehicle = _vehicleService.GetVehicle(vehicleId);
        if (vehicle == null || vehicle.Status != "Available")
        {
            Console.WriteLine("Fahrzeug nicht verfügbar!");
            return false;
        }

        // Prüfe Teilnehmer
        if (!_participantService.ParticipantExists(participantId))
        {
            Console.WriteLine("Teilnehmer nicht gefunden!");
            return false;
        }

        var booking = new Booking
        {
            BookingId = _nextId++,
            VehicleId = vehicleId,
            ParticipantId = participantId,
            StartTime = startTime,
            EndTime = endTime,
            BookingStatus = "Pending",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _bookings.Add(booking);

        // Starte Zahlungsprozess (automatisch bestätigt für Blazor)
        bool paymentSuccess = _paymentService.ProcessPayment(participantId, CalculatePrice(startTime, endTime), confirmPayment: true);

        if (paymentSuccess)
        {
            ConfirmBooking(booking.BookingId);
        }

        return paymentSuccess;
    }

    public void ConfirmBooking(int bookingId)
    {
        var booking = _bookings.FirstOrDefault(b => b.BookingId == bookingId);
        if (booking != null)
        {
            booking.BookingStatus = "Confirmed";
            booking.UpdatedAt = DateTime.Now;

            // Aktualisiere Fahrzeugstatus
            _vehicleService.UpdateVehicleStatus(booking.VehicleId, "Booked");

            Console.WriteLine($"Buchung {bookingId} bestätigt!");
        }
    }

    public List<Booking> GetUserBookings(int participantId)
    {
        return _bookings.Where(b => b.ParticipantId == participantId).ToList();
    }

    public List<Booking> GetAllBookings()
    {
        return _bookings.ToList();
    }

    private decimal CalculatePrice(DateTime start, DateTime end)
    {
        TimeSpan duration = end - start;
        return (decimal)(duration.TotalHours * 5.0); // 5€ pro Stunde
    }
}

