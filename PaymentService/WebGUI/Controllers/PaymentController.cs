using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using WebGUI.Models.Payments;

namespace WebGUI.Controllers;

public class PaymentController : Controller
{
    private readonly PaymentService _paymentService;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(PaymentService paymentService, ILogger<PaymentController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? participantId)
    {
        var vm = new PaymentPageViewModel
        {
            ParticipantId = participantId,
            Message = TempData["Message"] as string
        };

        if (participantId.HasValue && participantId.Value > 0)
        {
            vm.Payments = await _paymentService.GetPaymentHistoryAsync(participantId.Value);
        }

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PaymentPageViewModel model)
    {
        if (!model.ParticipantId.HasValue || model.ParticipantId.Value <= 0)
        {
            TempData["Message"] = "Bitte eine gültige ParticipantId angeben.";
            return RedirectToAction(nameof(Index));
        }

        if (!model.Amount.HasValue || model.Amount.Value <= 0)
        {
            TempData["Message"] = "Bitte einen gültigen Betrag angeben.";
            return RedirectToAction(nameof(Index), new { participantId = model.ParticipantId.Value });
        }

        var payment = await _paymentService.ProcessPaymentAsync(
            model.ParticipantId.Value,
            model.Amount.Value,
            model.ConfirmPayment);

        _logger.LogInformation("Payment created via WebGUI: {PaymentId} ({Status})", payment.PaymentId, payment.Status);
        TempData["Message"] = $"Zahlung erstellt: #{payment.PaymentId} ({payment.Status})";

        return RedirectToAction(nameof(Index), new { participantId = model.ParticipantId.Value });
    }
}



