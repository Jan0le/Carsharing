using Carsharing.Models.Entities;
using Carsharing.Services.Interfaces;
using Carsharing.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.Services.Implementations;

public class ParticipantService : IParticipantService
{
    private readonly ParticipantDbContext _context;

    public ParticipantService(ParticipantDbContext context)
    {
        _context = context;
        EnsureDatabaseCreated();
        SeedTestData();
    }

    private void EnsureDatabaseCreated()
    {
        try
        {
            _context.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            // Log error but don't throw - app should still start
            System.Diagnostics.Debug.WriteLine($"Database creation error: {ex.Message}");
        }
    }

    private void SeedTestData()
    {
        try
        {
            if (!_context.Participants.Any())
            {
                _context.Participants.AddRange(
                    new Participant
                    {
                        FirstName = "Max",
                        LastName = "Mustermann",
                        Email = "max@example.com",
                        BirthDate = new DateTime(1990, 5, 15),
                        Weight = 75.5m,
                        Height = 180.0m,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Participant
                    {
                        FirstName = "Anna",
                        LastName = "Schmidt",
                        Email = "anna@example.com",
                        BirthDate = new DateTime(1992, 8, 22),
                        Weight = 65.0m,
                        Height = 165.0m,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    }
                );
                _context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            // Log error but don't throw - app should still start
            System.Diagnostics.Debug.WriteLine($"Seed data error: {ex.Message}");
        }
    }

    public bool ParticipantExists(int participantId)
    {
        return _context.Participants.Any(p => p.ParticipantId == participantId);
    }

    public Participant? GetParticipant(int participantId)
    {
        return _context.Participants.FirstOrDefault(p => p.ParticipantId == participantId);
    }

    public List<Participant> GetAllParticipants()
    {
        return _context.Participants.ToList();
    }

    public void AddParticipant(Participant participant)
    {
        participant.CreatedAt = DateTime.Now;
        participant.UpdatedAt = DateTime.Now;
        _context.Participants.Add(participant);
        _context.SaveChanges();
    }

    public void UpdateParticipant(Participant participant)
    {
        var existing = _context.Participants.Find(participant.ParticipantId);
        if (existing != null)
        {
            existing.FirstName = participant.FirstName;
            existing.LastName = participant.LastName;
            existing.Email = participant.Email;
            existing.BirthDate = participant.BirthDate;
            existing.Weight = participant.Weight;
            existing.Height = participant.Height;
            existing.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
        }
    }

    public void DeleteParticipant(int participantId)
    {
        var participant = _context.Participants.Find(participantId);
        if (participant != null)
        {
            _context.Participants.Remove(participant);
            _context.SaveChanges();
        }
    }
}
