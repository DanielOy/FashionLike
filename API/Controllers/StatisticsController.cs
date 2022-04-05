using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StatisticsController(UserManager<User> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetUserStatistics")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StatisticDto>))]
        public async Task<ActionResult<IEnumerable<StatisticDto>>> GetUserStatistics()
        {
            var totals = await _userManager.Users
                .GroupBy(x => x.CreationDate.Date)
                .Select(x => new StatisticDto
                {
                    Date = x.Key.ToShortDateString(),
                    Count = x.Count()
                })
                .ToListAsync();

            return Ok(totals);
        }

        [HttpGet("GetLikeStatistics")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StatisticDto>))]
        public async Task<ActionResult<IEnumerable<StatisticDto>>> GetLikeStatistics()
        {
            var data = await GetStatisticsByReactionType(Core.Enums.ReactionType.Like);

            return Ok(data);
        }

        [HttpGet("GetDislikeStatistics")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StatisticDto>))]
        public async Task<ActionResult<IEnumerable<StatisticDto>>> GetDislikeStatistics()
        {
            var data = await GetStatisticsByReactionType(Core.Enums.ReactionType.Dislike);

            return Ok(data);
        }

        private async Task<List<StatisticDto>> GetStatisticsByReactionType(Core.Enums.ReactionType reaction)
        {
            var totals = await _unitOfWork.Reactions.GetAllByExpression(x => x.ReactionType == reaction);
            var data = totals.GroupBy(x => x.CreationDate.Date)
                .Select(x => new StatisticDto
                {
                    Date = x.Key.ToShortDateString(),
                    Count = x.Count()
                })
                .ToList();
            return data;
        }

        [HttpGet("GetLikesByCategoryStatistics")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReactionPostDto>))]
        public async Task<ActionResult<IEnumerable<ReactionPostDto>>> GetLikesByCategoryStatistics()
        {
            var categories = await _unitOfWork.Tags.GetAllByExpression();
            var reactionPosts = new List<ReactionPostDto>();

            foreach (var category in categories)
            {
                var post = new ReactionPostDto
                {
                    Category = category.Name
                };

                var postDto = await GetPostByCategoryAndReactionSort(category, "MostPopular");
                post.ImageUrl = postDto?.PictureUrl;
                post.ReactionCount = postDto?.LikeCount ?? 0;
                post.PostId = postDto?.Id ?? 0;
                post.PostDescription = postDto.Description;

                reactionPosts.Add(post);
            }

            return reactionPosts;
        }

        [HttpGet("GetDislikesByCategoryStatistics")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReactionPostDto>))]
        public async Task<ActionResult<IEnumerable<ReactionPostDto>>> GetDislikesByCategoryStatistics()
        {
            var categories = await _unitOfWork.Tags.GetAllByExpression();
            var reactionPosts = new List<ReactionPostDto>();

            foreach (var category in categories)
            {
                var post = new ReactionPostDto
                {
                    Category = category.Name
                };

                var postDto = await GetPostByCategoryAndReactionSort(category, "LeastPopular");
                post.ImageUrl = postDto?.PictureUrl;
                post.ReactionCount = postDto?.DislikeCount ?? 0;
                post.PostId = postDto?.Id ?? 0;
                post.PostDescription = postDto.Description;

                reactionPosts.Add(post);
            }

            return reactionPosts;
        }

        private async Task<PostDto> GetPostByCategoryAndReactionSort(Tag category, string sort)
        {
            var postParams = new PostSpecParams
            {
                PageIndex = 1,
                PageSize = 1,
                Tag = category.Name,
                Sort = sort
            };
            var spec = new PostPaginationSpecification(postParams);
            var posts = await _unitOfWork.Posts.GetAllBySpecification(spec);
            var data = _mapper.Map<IEnumerable<Post>, IEnumerable<PostDto>>(posts);
            return data.FirstOrDefault();
        }
    }
}
