using Core.Entities;

namespace Core.Specifications
{
    public class PostReactionSpecification : BaseSpecification<Reaction>
    {
        public PostReactionSpecification(int postId, string userId) : base(x => x.PostId == postId && x.UserId == userId)
        {
        }
    }
}
