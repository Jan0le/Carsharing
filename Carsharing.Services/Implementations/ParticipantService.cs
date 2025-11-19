using Carsharing.Models.Entities;
using Carsharing.Services.Interfaces;

namespace Carsharing.Services.Implementations;

public class ParticipantService : IParticipantService
{
    private List<Participant> _participants = new();
    private int _nextId = 1;

    public ParticipantService()
    {
        // Testdaten
        _participants.Add(new Participant { 
            ParticipantId = _nextId++, 
            FirstName = "Max", 
            LastName = "Mustermann", 
            Email = "max@example.com",
            CreatedAt = DateTime.Now
        });

        _participants.Add(new Participant { 
            ParticipantId = _nextId++, 
            FirstName = "Anna", 
            LastName = "Schmidt", 
            Email = "anna@example.com",
            CreatedAt = DateTime.Now
        });
    }

    public bool ParticipantExists(int participantId)
    {
        return _participants.Any(p => p.ParticipantId == participantId);
    }

    public Participant? GetParticipant(int participantId)
    {
        return _participants.FirstOrDefault(p => p.ParticipantId == participantId);
    }

    public List<Participant> GetAllParticipants()
    {
        return _participants;
    }

    public void AddParticipant(Participant participant)
    {
        participant.ParticipantId = _nextId++;
        participant.CreatedAt = DateTime.Now;
        _participants.Add(participant);
    }
}

