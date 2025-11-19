using Carsharing.Models.Entities;

namespace Carsharing.Services.Interfaces;

public interface IParticipantService
{
    bool ParticipantExists(int participantId);
    Participant? GetParticipant(int participantId);
    List<Participant> GetAllParticipants();
    void AddParticipant(Participant participant);
}

