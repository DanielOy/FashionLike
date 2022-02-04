using Core.Entities;

namespace Core.Specifications
{
    public class PostByIdWithDetailsSpecification : BaseSpecification<Post>
    {
        public PostByIdWithDetailsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.User);
            AddInclude(x => x.Tags);
        }
    }
}
