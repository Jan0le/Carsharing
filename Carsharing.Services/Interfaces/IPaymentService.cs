namespace Carsharing.Services.Interfaces;

public interface IPaymentService
{
    bool ProcessPayment(int participantId, decimal amount);
}

