namespace LogNow.API.Models;

public class Service
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string OwnerTeam { get; set; } = string.Empty;
    public ServiceStatus Status { get; set; } = ServiceStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
}

public enum ServiceStatus
{
    Active,
    Inactive,
    Deprecated
}
