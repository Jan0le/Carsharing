using Carsharing.Models.Entities;

namespace Carsharing.Controllers.Mvc;

public static class ParticipantView
{
    public static void DisplayParticipants(List<Participant> participants)
    {
        Console.WriteLine("\n=== Teilnehmer ===");
        foreach (var participant in participants)
        {
            Console.WriteLine($"ID: {participant.ParticipantId} | {participant.FirstName} {participant.LastName} | {participant.Email}");
        }
    }

    public static Participant GetNewParticipantDetails()
    {
        Console.WriteLine("\n=== Neuer Teilnehmer ===");
        Console.Write("Vorname: ");
        string firstName = Console.ReadLine() ?? string.Empty;
        Console.Write("Nachname: ");
        string lastName = Console.ReadLine() ?? string.Empty;
        Console.Write("Email: ");
        string email = Console.ReadLine() ?? string.Empty;

        return new Participant 
        { 
            FirstName = firstName, 
            LastName = lastName, 
            Email = email 
        };
    }

    public static void ShowParticipantMenu()
    {
        Console.WriteLine("\n=== ParticipantService Menu ===");
        Console.WriteLine("1. Alle Teilnehmer anzeigen");
        Console.WriteLine("2. Neuen Teilnehmer hinzufügen");
        Console.WriteLine("3. Zurück zum Hauptmenü");
    }
}

