using Carsharing.Services.Interfaces;

namespace Carsharing.Controllers.Mvc;

public class BookingController
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public void CreateNewBooking()
    {
        var bookingDetails = BookingView.GetBookingDetails();
        bool success = _bookingService.CreateBooking(
            bookingDetails.VehicleId,
            bookingDetails.ParticipantId,
            bookingDetails.StartTime,
            bookingDetails.EndTime
        );

        if (success)
        {
            Console.WriteLine("Buchung erfolgreich erstellt und bezahlt!");
        }
        else
        {
            Console.WriteLine("Buchung fehlgeschlagen!");
        }
    }

    public void ShowUserBookings(int participantId)
    {
        var bookings = _bookingService.GetUserBookings(participantId);
        BookingView.DisplayBookings(bookings);
    }
}

