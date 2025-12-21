using System.ComponentModel.DataAnnotations;

namespace RestApi.Dtos;

public class CreatePaymentDto
{
    [Required]
    public int ParticipantId { get; set; }

    [Range(0.01, 1_000_000)]
    public decimal Amount { get; set; }

    public bool ConfirmPayment { get; set; } = true;
}

public class ReadPaymentDto
{
    public int PaymentId { get; set; }
    public int ParticipantId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}



