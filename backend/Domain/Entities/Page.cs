using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raras.EMS.API.Models.Entities;

[Table("pages")]
public class Page
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("module_id")]
    public int ModuleId { get; set; }

    [ForeignKey(nameof(ModuleId))]
    public Module? Module { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("key")]
    public string Key { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    [Column("display_name")]
    public string DisplayName { get; set; } = string.Empty;

    [MaxLength(255)]
    [Column("route_path")]
    public string? RoutePath { get; set; }

    [Column("sort_order")]
    public int SortOrder { get; set; } = 0;

    public ICollection<Functionality> Functionalities { get; set; } = new List<Functionality>();
}
