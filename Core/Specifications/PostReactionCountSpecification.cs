using Core.Entities;

namespace Core.Specifications
{
    public class PostReactionCountSpecification : BaseSpecification<Reaction>
    {
        public PostReactionCountSpecification(int postId) : base(x => x.PostId == postId)
        {
        }
    }
}
