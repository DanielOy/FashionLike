using Core.Entities;

namespace Core.Specifications
{
    public class GetTagByNameSpecification : BaseSpecification<Tag>
    {
        public GetTagByNameSpecification(string name) : base(x => x.Name == name)
        {
        }
    }
}
