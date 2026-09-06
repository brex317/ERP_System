namespace Raras.EMS.API.Models.DTOs;

public class DashboardStatsDto
{
    public int TotalEmployees { get; set; }
    public int TotalDepartments { get; set; }
    public int PresentToday { get; set; }
    public int OnLeave { get; set; }
}
