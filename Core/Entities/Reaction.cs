using Core.Enums;

namespace Core.Entities
{
    public class Reaction
    {
        public int Id { get; set; }
        public Post Post { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ReactionType ReactionType { get; set; }
    }
}
