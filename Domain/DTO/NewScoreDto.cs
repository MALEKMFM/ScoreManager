using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HighscoreManager.Domain.DTO;
public class NewScoreDto
{
   public int GameId { get; set; }
   public virtual GameDto? Game { get; set; }
   public string PlayerName { get; set; }
   public DateTime HighscoreDate { get; set; }
  public int Points { get; set; }
}
