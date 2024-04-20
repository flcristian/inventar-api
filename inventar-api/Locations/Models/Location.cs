using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using inventar_api.ArticleLocations.Models;

namespace inventar_api.Locations.Models;

[Table("locations")]
public class Location
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [Column("code")]
    public string Code { get; set; }
        
    public virtual List<ArticleLocation> ArticleLocations { get; set; }


}