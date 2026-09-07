namespace Raras.EMS.API.Models.DTOs;

public class HelpStepDto
{
    public int Number { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class HelpResponseDto
{
    public string ModuleKey { get; set; } = string.Empty;
    public string FeatureKey { get; set; } = string.Empty;
    public string FeatureSpecificationKey { get; set; } = string.Empty;
    public string Title { get; set; } = "Quick steps";
    public List<HelpStepDto> Steps { get; set; } = new();
}
