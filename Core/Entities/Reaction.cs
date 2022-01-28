using Core.Enums;

namespace Core.Entities
{
    public class Reaction
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ReactionType ReactionType { get; set; }
    }
}
