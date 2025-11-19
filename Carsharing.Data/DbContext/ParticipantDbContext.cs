using Microsoft.EntityFrameworkCore;
using Carsharing.Models.Entities;

namespace Carsharing.Data.DbContext;

public class ParticipantDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ParticipantDbContext(DbContextOptions<ParticipantDbContext> options) : base(options)
    {
    }

    public DbSet<Participant> Participants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Participant>(entity =>
        {
            entity.ToTable("Participants");
            
            entity.HasKey(e => e.ParticipantId);
            
            entity.Property(e => e.ParticipantId)
                .HasColumnName("ParticipantId")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(100)
                .IsRequired();
            
            entity.Property(e => e.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(100)
                .IsRequired();
            
            entity.Property(e => e.Email)
                .HasColumnName("Email")
                .HasMaxLength(255)
                .IsRequired();
            
            entity.Property(e => e.BirthDate)
                .HasColumnName("BirthDate");
            
            entity.Property(e => e.Weight)
                .HasColumnName("Weight");
            
            entity.Property(e => e.Height)
                .HasColumnName("Height");
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();
            
            entity.Property(e => e.UpdatedAt)
                .HasColumnName("UpdatedAt")
                .IsRequired();
        });
    }
}

