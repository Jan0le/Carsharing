using Carsharing.Models.Entities;

namespace Carsharing.Services.Interfaces;

public interface IPaymentService
{
    bool ProcessPayment(int participantId, decimal amount);
    List<Payment> GetPaymentHistory(int participantId);
}

