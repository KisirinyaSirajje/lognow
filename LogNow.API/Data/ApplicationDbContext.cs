using LogNow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogNow.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Incident> Incidents { get; set; }
    public DbSet<IncidentComment> IncidentComments { get; set; }
    public DbSet<IncidentTimeline> IncidentTimelines { get; set; }
    public DbSet<SLA> SLAs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.Username).IsUnique();
            entity.Property(e => e.Role).HasConversion<string>();
        });

        // Service configuration
        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).HasConversion<string>();
        });

        // Incident configuration
        modelBuilder.Entity<Incident>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.IncidentNumber).IsUnique();
            entity.Property(e => e.Severity).HasConversion<string>();
            entity.Property(e => e.Status).HasConversion<string>();

            entity.HasOne(e => e.Service)
                .WithMany(s => s.Incidents)
                .HasForeignKey(e => e.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CreatedByUser)
                .WithMany(u => u.CreatedIncidents)
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.AssignedToUser)
                .WithMany(u => u.AssignedIncidents)
                .HasForeignKey(e => e.AssignedToUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.AssignedByUser)
                .WithMany()
                .HasForeignKey(e => e.AssignedByUserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // IncidentComment configuration
        modelBuilder.Entity<IncidentComment>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Incident)
                .WithMany(i => i.Comments)
                .HasForeignKey(e => e.IncidentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // IncidentTimeline configuration
        modelBuilder.Entity<IncidentTimeline>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ActionType).HasConversion<string>();

            entity.HasOne(e => e.Incident)
                .WithMany(i => i.Timeline)
                .HasForeignKey(e => e.IncidentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany(u => u.TimelineActions)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // SLA configuration
        modelBuilder.Entity<SLA>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Severity).HasConversion<string>();
            entity.HasIndex(e => e.Severity).IsUnique();
        });

        // Seed default SLAs
        var defaultSLAs = SLA.GetDefaultSLAs();
        modelBuilder.Entity<SLA>().HasData(
            new SLA { Id = Guid.NewGuid(), Severity = Severity.SEV1, ResponseTimeMinutes = 5, ResolutionTimeMinutes = 30 },
            new SLA { Id = Guid.NewGuid(), Severity = Severity.SEV2, ResponseTimeMinutes = 15, ResolutionTimeMinutes = 120 },
            new SLA { Id = Guid.NewGuid(), Severity = Severity.SEV3, ResponseTimeMinutes = 60, ResolutionTimeMinutes = 1440 },
            new SLA { Id = Guid.NewGuid(), Severity = Severity.SEV4, ResponseTimeMinutes = 240, ResolutionTimeMinutes = 4320 }
        );
    }
}
