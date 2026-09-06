using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raras.EMS.API.Models.Entities;

[Table("functionalities")]
public class Functionality
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("page_id")]
    public int PageId { get; set; }

    [ForeignKey(nameof(PageId))]
    public Page? Page { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("key")]
    public string Key { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    [Column("display_name")]
    public string DisplayName { get; set; } = string.Empty;
}
