using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class PostCountSpecification : BaseSpecification<Post>
    {
        public PostCountSpecification(PostSpecParams postSpecParams) : base(x =>
        (string.IsNullOrEmpty(postSpecParams.Search) || x.Description.ToLower().Contains(postSpecParams.Search)))
        { }
    }
}
