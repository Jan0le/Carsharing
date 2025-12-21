using System.ComponentModel.DataAnnotations;

namespace Carsharing.Models.Entities;

public class Participant
{
    public int ParticipantId { get; set; }

    [Required(ErrorMessage = "Vorname ist erforderlich.")]
    [StringLength(100, ErrorMessage = "Vorname ist zu lang.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nachname ist erforderlich.")]
    [StringLength(100, ErrorMessage = "Nachname ist zu lang.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email ist erforderlich.")]
    [EmailAddress(ErrorMessage = "Bitte eine gültige Email angeben.")]
    [StringLength(200, ErrorMessage = "Email ist zu lang.")]
    public string Email { get; set; } = string.Empty;

    public DateTime? BirthDate { get; set; }
    public decimal? Weight { get; set; }
    public decimal? Height { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

