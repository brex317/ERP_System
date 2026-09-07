namespace Raras.EMS.API.Models.DTOs;

public class SearchResultItemDto
{
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string LinkUrl { get; set; } = string.Empty;
}

public class GlobalSearchResponseDto
{
    public string Query { get; set; } = string.Empty;
    public List<SearchResultItemDto> Results { get; set; } = new();
}
