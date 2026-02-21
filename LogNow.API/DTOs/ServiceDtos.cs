namespace LogNow.API.DTOs;

public class ServiceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string OwnerTeam { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateServiceDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string OwnerTeam { get; set; } = string.Empty;
    public string Status { get; set; } = "Active";
}

public class UpdateServiceDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? OwnerTeam { get; set; }
    public string? Status { get; set; }
}
