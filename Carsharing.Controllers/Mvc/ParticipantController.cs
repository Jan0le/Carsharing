using Carsharing.Services.Interfaces;

namespace Carsharing.Controllers.Mvc;

public class ParticipantController
{
    private readonly IParticipantService _participantService;

    public ParticipantController(IParticipantService participantService)
    {
        _participantService = participantService;
    }

    public void ShowAllParticipants()
    {
        var participants = _participantService.GetAllParticipants();
        ParticipantView.DisplayParticipants(participants);
    }

    public void AddNewParticipant()
    {
        var participant = ParticipantView.GetNewParticipantDetails();
        _participantService.AddParticipant(participant);
        Console.WriteLine("Neuer Teilnehmer hinzugefügt!");
    }
}

