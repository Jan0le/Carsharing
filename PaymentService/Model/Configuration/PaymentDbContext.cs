using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Model.Configuration;

public class PaymentDbContext : DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
    {
    }

    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payments");

            entity.HasKey(e => e.PaymentId);

            entity.Property(e => e.PaymentId)
                .HasColumnName("PaymentId")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.ParticipantId)
                .HasColumnName("ParticipantId")
                .IsRequired();

            entity.Property(e => e.Amount)
                .HasColumnName("Amount")
                .HasPrecision(18, 2)
                .IsRequired();

            entity.Property(e => e.Status)
                .HasColumnName("Status")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();
        });
    }
}



