using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ReactionDto
    {
        [Required]
        public int PostId { get; set; }
        [Required]
        public int ReactionType { get; set; }
    }
}
