using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace HighscoreManager.Domain.DTO;

public class ScoreDto
{
   public int Id { get; set; }

   [Display(Name = "Game")]
   public int GameId { get; set; }

   [ForeignKey("GameId")]
   public virtual GameDto? GameDto { get; set; }

   [Required]
   [Display(Name = "Player name")]
   public string PlayerName { get; set; }

   [Required]
   [Display(Name = "Date")]
   public DateTime HighscoreDate { get; set; }

   [Required]
   [Display(Name = "Points")]
   public int Points { get; set; }
}