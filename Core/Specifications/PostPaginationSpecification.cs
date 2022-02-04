using Core.Entities;

namespace Core.Specifications
{
    public class PostPaginationSpecification : BaseSpecification<Post>
    {
        public PostPaginationSpecification(PostSpecParams postSpecParams) : base(x =>
        (string.IsNullOrEmpty(postSpecParams.Search) || x.Description.ToLower().Contains(postSpecParams.Search)))
        {
            ApplyPagging(postSpecParams.PageSize * (postSpecParams.PageIndex - 1), postSpecParams.PageSize);

            if (!string.IsNullOrWhiteSpace(postSpecParams.Sort))
            {
                switch (postSpecParams.Sort)
                {
                    case "DateAsc":
                        AddOrderBy(x => x.CreationDate);
                        break;
                    case "DateDesc":
                        AddOrderByDescending(x => x.CreationDate);
                        break;
                    default:
                        AddOrderBy(x => x.Id);
                        break;
                }
            }

            AddInclude(x => x.User);
            AddInclude(x => x.Tags);
        }
    }
}
