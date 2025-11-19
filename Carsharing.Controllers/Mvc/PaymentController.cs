using Carsharing.Services.Interfaces;

namespace Carsharing.Controllers.Mvc;

public class PaymentController
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public void ShowPaymentHistory(int participantId)
    {
        var payments = _paymentService.GetPaymentHistory(participantId);
        PaymentView.DisplayPayments(payments);
    }
}

