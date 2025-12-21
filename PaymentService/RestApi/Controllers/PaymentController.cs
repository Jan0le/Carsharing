using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using RestApi.Dtos;

namespace RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly PaymentService _paymentService;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(PaymentService paymentService, ILogger<PaymentController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<ReadPaymentDto>> CreateAsync([FromBody] CreatePaymentDto dto)
    {
        var payment = await _paymentService.ProcessPaymentAsync(dto.ParticipantId, dto.Amount, dto.ConfirmPayment);
        _logger.LogInformation("Created payment {PaymentId} for participant {ParticipantId} ({Status})", payment.PaymentId, payment.ParticipantId, payment.Status);

        return CreatedAtAction(nameof(GetByIdAsync), new { id = payment.PaymentId }, payment.ToReadDto());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadPaymentDto>> GetByIdAsync(int id)
    {
        var payment = await _paymentService.GetByIdAsync(id);
        if (payment is null)
        {
            return NotFound();
        }

        return Ok(payment.ToReadDto());
    }

    [HttpGet]
    public async Task<ActionResult<List<ReadPaymentDto>>> GetHistoryAsync([FromQuery] int participantId)
    {
        var payments = await _paymentService.GetPaymentHistoryAsync(participantId);
        return Ok(payments.Select(p => p.ToReadDto()).ToList());
    }
}

internal static class PaymentDtoMapper
{
    public static ReadPaymentDto ToReadDto(this Payment payment) => new()
    {
        PaymentId = payment.PaymentId,
        ParticipantId = payment.ParticipantId,
        Amount = payment.Amount,
        Status = payment.Status,
        CreatedAt = payment.CreatedAt
    };
}


