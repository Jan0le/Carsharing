using Model.Entities;

namespace WebGUI.Models.Payments;

public class PaymentPageViewModel
{
    public int? ParticipantId { get; set; }
    public decimal? Amount { get; set; }
    public bool ConfirmPayment { get; set; } = true;

    public string? Message { get; set; }

    public List<Payment> Payments { get; set; } = new();
}



