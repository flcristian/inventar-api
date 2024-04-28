using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using inventar_api.ArticleLocations.Models;

namespace inventar_api.Articles.Models;

[Table("articles")]
public class Article
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [Column("code")]
    public int Code { get; set; }
    
    [Required]
    [Column("name")]
    public string Name { get; set; }

    public virtual List<ArticleLocation> ArticleLocations { get; set; }
    public virtual List<ArticleLocationHistory> ArticleLocationHistory { get; set; }
}