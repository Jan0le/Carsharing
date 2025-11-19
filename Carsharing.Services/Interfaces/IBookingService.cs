using Carsharing.Models.Entities;

namespace Carsharing.Services.Interfaces;

public interface IBookingService
{
    bool CreateBooking(int vehicleId, int participantId, DateTime startTime, DateTime endTime);
    void ConfirmBooking(int bookingId);
    List<Booking> GetUserBookings(int participantId);
}

