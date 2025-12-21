using Domain.Services;
using Domain.Tests.TestDoubles;
using Model.Entities;

namespace Domain.Tests;

public class PaymentServiceTests
{
    private static Domain.Services.PaymentService CreateSut(out FakeRepositoryAsync<Payment> repo)
    {
        repo = new FakeRepositoryAsync<Payment>(
            getId: p => p.PaymentId,
            setId: (p, id) => p.PaymentId = id);

        return new Domain.Services.PaymentService(repo);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task ProcessPaymentAsync_InvalidParticipantId_Throws(int participantId)
    {
        var sut = CreateSut(out _);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            sut.ProcessPaymentAsync(participantId, 10m, confirmPayment: true));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-0.01)]
    public async Task ProcessPaymentAsync_InvalidAmount_Throws(decimal amount)
    {
        var sut = CreateSut(out _);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            sut.ProcessPaymentAsync(1, amount, confirmPayment: true));
    }

    [Fact]
    public async Task ProcessPaymentAsync_ConfirmTrue_SetsCompleted()
    {
        var sut = CreateSut(out _);

        var payment = await sut.ProcessPaymentAsync(1, 12.50m, confirmPayment: true);

        Assert.True(payment.PaymentId > 0);
        Assert.Equal(1, payment.ParticipantId);
        Assert.Equal(12.50m, payment.Amount);
        Assert.Equal("Completed", payment.Status);
        Assert.True(payment.CreatedAt > DateTime.MinValue);
    }

    [Fact]
    public async Task ProcessPaymentAsync_ConfirmFalse_SetsFailed()
    {
        var sut = CreateSut(out _);

        var payment = await sut.ProcessPaymentAsync(1, 12.50m, confirmPayment: false);

        Assert.Equal("Failed", payment.Status);
    }

    [Fact]
    public async Task GetPaymentHistoryAsync_ReturnsPaymentsForParticipant()
    {
        var sut = CreateSut(out _);

        await sut.ProcessPaymentAsync(1, 5m, confirmPayment: true);
        await sut.ProcessPaymentAsync(2, 7m, confirmPayment: true);
        await sut.ProcessPaymentAsync(1, 9m, confirmPayment: false);

        var history = await sut.GetPaymentHistoryAsync(1);

        Assert.Equal(2, history.Count);
        Assert.All(history, p => Assert.Equal(1, p.ParticipantId));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetPaymentHistoryAsync_InvalidParticipantId_Throws(int participantId)
    {
        var sut = CreateSut(out _);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            sut.GetPaymentHistoryAsync(participantId));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsPaymentOrNull()
    {
        var sut = CreateSut(out _);

        var created = await sut.ProcessPaymentAsync(1, 3m, confirmPayment: true);
        var found = await sut.GetByIdAsync(created.PaymentId);
        var missing = await sut.GetByIdAsync(created.PaymentId + 999);

        Assert.NotNull(found);
        Assert.Equal(created.PaymentId, found!.PaymentId);
        Assert.Null(missing);
    }
}


