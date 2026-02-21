using LogNow.API.Models;
using LogNow.API.Data;
using Microsoft.EntityFrameworkCore;

namespace LogNow.API.Services;

public class DataSeedService
{
    private readonly ApplicationDbContext _context;

    public DataSeedService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedDefaultServicesAsync()
    {
        // Check if services already exist
        if (await _context.Services.AnyAsync())
        {
            return; // Services already seeded
        }

        var defaultServices = new List<Service>
        {
            new Service
            {
                Id = Guid.NewGuid(),
                Name = "API Gateway",
                Description = "Central API gateway handling all external API requests",
                OwnerTeam = "Platform Team",
                Status = ServiceStatus.Active,
                CreatedAt = DateTime.UtcNow
            },
            new Service
            {
                Id = Guid.NewGuid(),
                Name = "Authentication Service",
                Description = "User authentication and authorization service",
                OwnerTeam = "Security Team",
                Status = ServiceStatus.Active,
                CreatedAt = DateTime.UtcNow
            },
            new Service
            {
                Id = Guid.NewGuid(),
                Name = "Database Cluster",
                Description = "Primary PostgreSQL database cluster",
                OwnerTeam = "Database Team",
                Status = ServiceStatus.Active,
                CreatedAt = DateTime.UtcNow
            },
            new Service
            {
                Id = Guid.NewGuid(),
                Name = "Payment Processing",
                Description = "Payment gateway and transaction processing",
                OwnerTeam = "Finance Team",
                Status = ServiceStatus.Active,
                CreatedAt = DateTime.UtcNow
            },
            new Service
            {
                Id = Guid.NewGuid(),
                Name = "Email Service",
                Description = "Email delivery and notification service",
                OwnerTeam = "Communication Team",
                Status = ServiceStatus.Active,
                CreatedAt = DateTime.UtcNow
            },
            new Service
            {
                Id = Guid.NewGuid(),
                Name = "File Storage",
                Description = "Cloud file storage and CDN service",
                OwnerTeam = "Infrastructure Team",
                Status = ServiceStatus.Active,
                CreatedAt = DateTime.UtcNow
            },
            new Service
            {
                Id = Guid.NewGuid(),
                Name = "Search Service",
                Description = "Elasticsearch-based search and analytics",
                OwnerTeam = "Data Team",
                Status = ServiceStatus.Active,
                CreatedAt = DateTime.UtcNow
            },
            new Service
            {
                Id = Guid.NewGuid(),
                Name = "Caching Layer",
                Description = "Redis distributed caching service",
                OwnerTeam = "Platform Team",
                Status = ServiceStatus.Active,
                CreatedAt = DateTime.UtcNow
            },
            new Service
            {
                Id = Guid.NewGuid(),
                Name = "Mobile Backend",
                Description = "Backend services for mobile applications",
                OwnerTeam = "Mobile Team",
                Status = ServiceStatus.Active,
                CreatedAt = DateTime.UtcNow
            },
            new Service
            {
                Id = Guid.NewGuid(),
                Name = "Analytics Pipeline",
                Description = "Data analytics and reporting pipeline",
                OwnerTeam = "Analytics Team",
                Status = ServiceStatus.Active,
                CreatedAt = DateTime.UtcNow
            }
        };

        await _context.Services.AddRangeAsync(defaultServices);
        await _context.SaveChangesAsync();
    }
}
