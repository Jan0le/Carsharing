using Domain.Interfaces;
using Model.Entities;

namespace Domain.Services;

public class PaymentService
{
    private readonly IRepositoryAsync<Payment> _paymentRepository;

    public PaymentService(IRepositoryAsync<Payment> paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Payment> ProcessPaymentAsync(int participantId, decimal amount, bool confirmPayment = true)
    {
        if (participantId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(participantId), "ParticipantId must be greater than zero.");
        }

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
        }

        var success = confirmPayment;

        var payment = new Payment
        {
            ParticipantId = participantId,
            Amount = amount,
            Status = success ? "Completed" : "Failed",
            CreatedAt = DateTime.UtcNow
        };

        await _paymentRepository.CreateAsync(payment);
        return payment;
    }

    public Task<List<Payment>> GetPaymentHistoryAsync(int participantId)
    {
        if (participantId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(participantId), "ParticipantId must be greater than zero.");
        }

        return _paymentRepository.ReadAsync(p => p.ParticipantId == participantId);
    }

    public Task<Payment?> GetByIdAsync(int paymentId)
    {
        if (paymentId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(paymentId), "PaymentId must be greater than zero.");
        }

        return _paymentRepository.ReadAsync(paymentId);
    }

    public async Task<List<int>> GetAvailableParticipantIdsAsync()
    {
        var allPayments = await _paymentRepository.ReadAllAsync();
        return allPayments
            .Select(p => p.ParticipantId)
            .Distinct()
            .OrderBy(id => id)
            .ToList();
    }
}


