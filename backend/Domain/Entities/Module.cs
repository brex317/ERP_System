using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raras.EMS.API.Models.Entities;

[Table("modules")]
public class Module
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("key")]
    public string Key { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    [Column("display_name")]
    public string DisplayName { get; set; } = string.Empty;

    [MaxLength(50)]
    [Column("icon")]
    public string? Icon { get; set; }

    [Column("sort_order")]
    public int SortOrder { get; set; } = 0;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    public ICollection<Page> Pages { get; set; } = new List<Page>();
}
