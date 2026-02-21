namespace LogNow.API.DTOs;

public class DashboardDto
{
    public int TotalIncidents { get; set; }
    public int OpenIncidents { get; set; }
    public int InProgressIncidents { get; set; }
    public int ResolvedIncidents { get; set; }
    
    public Dictionary<string, int> IncidentsBySeverity { get; set; } = new();
    public Dictionary<string, int> IncidentsByService { get; set; } = new();
    public Dictionary<string, int> IncidentsByStatus { get; set; } = new();
    
    public List<IncidentDto> RecentIncidents { get; set; } = new();
    public int SlaBreaches { get; set; }
}
