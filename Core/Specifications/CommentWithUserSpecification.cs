using Core.Entities;
using System;

namespace Core.Specifications
{
    public class CommentWithUserSpecification : BaseSpecification<Comment>
    {
        public CommentWithUserSpecification(Guid id) : base(x => x.Id == id)
        {
            AddInclude(x => x.User);
        }
    }
}
