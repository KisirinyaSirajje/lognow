namespace LogNow.API.Models;

public class SLA
{
    public Guid Id { get; set; }
    public Severity Severity { get; set; }
    public int ResponseTimeMinutes { get; set; }
    public int ResolutionTimeMinutes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public static Dictionary<Severity, SLA> GetDefaultSLAs()
    {
        return new Dictionary<Severity, SLA>
        {
            { Severity.SEV1, new SLA { Severity = Severity.SEV1, ResponseTimeMinutes = 5, ResolutionTimeMinutes = 30 } },
            { Severity.SEV2, new SLA { Severity = Severity.SEV2, ResponseTimeMinutes = 15, ResolutionTimeMinutes = 120 } },
            { Severity.SEV3, new SLA { Severity = Severity.SEV3, ResponseTimeMinutes = 60, ResolutionTimeMinutes = 1440 } },
            { Severity.SEV4, new SLA { Severity = Severity.SEV4, ResponseTimeMinutes = 240, ResolutionTimeMinutes = 4320 } }
        };
    }
}
