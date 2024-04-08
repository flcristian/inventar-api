using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using inventar_api.Articles.Models;
using inventar_api.Locations.Models;

namespace inventar_api.ArticleLocations.Models;

[Table("articleLocations")]
public class ArticleLocation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [Column("articleId")]
    public int ArticleId { get; set; }
    
    [JsonIgnore]
    public virtual Article Article { get; set; }
    
    [Required]
    [Column("locationId")]
    public int LocationId { get; set; }
    
    [JsonIgnore]
    public virtual Location Location { get; set; }
    
    [Required]
    [Column("count")]
    public int Count { get; set; }
}