using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HighscoreManager.Domain.DTO;

public class GameDto
{
   [NotMapped]
   public List<ScoreDto> HighScores { get; set; }
   public int Id { get; set; }
   public string Name { get; set; }
   public string Description { get; set; }
   public DateTime ReleaseDate { get; set; }
   public Uri ImageUrl { get; set; }

   [MaxLength(50)]
   public string? UrlSlug { get; set; }
}

