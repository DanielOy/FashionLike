using API.Dtos;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _environmet;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImagesService _imagesService;
        private readonly IMapper _mapper;

        public PostsController(IWebHostEnvironment environmet,
            UserManager<User> userManager,
            IUnitOfWork unitOfWork, IImagesService imagesService, IMapper mapper)
        {
            _environmet = environmet;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _imagesService = imagesService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,Viewer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Pagination<PostDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<Pagination<PostDto>>> Get([FromQuery] PostSpecParams postParams)
        {
            var spec = new PostPaginationSpecification(postParams);

            var countSpec = new PostCountSpecification(postParams);

            var total = await _unitOfWork.Posts.CountAsync(countSpec);

            var posts = await _unitOfWork.Posts.GetAllBySpecification(spec);

            var data = _mapper.Map<IEnumerable<Post>, IEnumerable<PostDto>>(posts);

            var page = new Pagination<PostDto>(postParams.PageIndex, postParams.PageSize, total, data);

            return Ok(page);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator,Viewer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<PostDto>> Get(int id)
        {
            var spec = new PostByIdWithDetailsSpecification(id);
            var post = await _unitOfWork.Posts.GetBySpecification(spec);
            if (post is null) return NotFound();

            var postDto = _mapper.Map<Post, PostDto>(post);

            return Ok(postDto);
        }

        private async Task<Reaction> GetCurrentUserReactionFromPost(int postId)
        {
            string userId = await User.GetCurrentUserId(_userManager);

            var userReactionSpec = new PostReactionSpecification(postId, userId);
            var userReaction = await _unitOfWork.Reactions.GetBySpecification(userReactionSpec);

            return userReaction;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<PostDto>> Post([FromBody] PostToCreateDto postToCreateDto)
        {
            string userId = await User.GetCurrentUserId(_userManager);

            string normalizedName = _imagesService.GetNormalizedName(postToCreateDto.FileName);

            await _imagesService.StorageImage(postToCreateDto.ImageBase64, normalizedName, _environmet.ContentRootPath);

            var tags = await SaveTags(postToCreateDto.Tags);

            var post = new Post
            {
                Active = true,
                CreationDate = DateTime.Now,
                Description = postToCreateDto.Description,
                UserId = userId,
                PictureName = normalizedName,
                Tags = tags.ToList()
            };

            _unitOfWork.Posts.Insert(post);
            await _unitOfWork.Save();

            var postDto = _mapper.Map<Post, PostDto>(post);

            return Ok(postDto);
        }

        private async Task<IEnumerable<Tag>> SaveTags(List<string> tags)
        {
            await SaveNewTags(tags);
            return await GetTagsByNames(tags);
        }

        private async Task<IEnumerable<Tag>> GetTagsByNames(List<string> tagNames)
        {
            List<Tag> tags = new();
            foreach (var tagName in tagNames)
            {
                var spec = new GetTagByNameSpecification(tagName);
                var existingTag = await _unitOfWork.Tags.GetBySpecification(spec);

                if (existingTag != null)
                {
                    tags.Add(existingTag);
                }
            }
            return tags;
        }

        private async Task SaveNewTags(List<string> tags)
        {
            foreach (var tag in tags)
            {
                var spec = new GetTagByNameSpecification(tag);
                var existingTag = await _unitOfWork.Tags.GetBySpecification(spec);

                if (existingTag is null)
                {
                    _unitOfWork.Tags.Insert(new Tag { Name = tag });
                }
            }
            await _unitOfWork.Save();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<PostDto>> Put(int id, [FromBody] PostToUpdateDto postToUpdateDto)
        {
            var post = await _unitOfWork.Posts.GetByID(id);
            post.Description = postToUpdateDto.Description;

            if (!string.IsNullOrEmpty(postToUpdateDto.ImageBase64))
            {
                string normalizedName = _imagesService.GetNormalizedName(postToUpdateDto.FileName);
                await _imagesService.StorageImage(postToUpdateDto.ImageBase64, normalizedName, _environmet.ContentRootPath);
                post.PictureName = normalizedName;
            }

            if (postToUpdateDto.Tags?.Count > 0)
            {
                var tags = await SaveTags(postToUpdateDto.Tags);
                post.Tags = tags.ToList();
            }

            _unitOfWork.Posts.Update(post);
            await _unitOfWork.Save();

            var updatedPost = _mapper.Map<Post, PostDto>(post);

            return Ok(updatedPost);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            _unitOfWork.Posts.DeleteByID(id);

            await _unitOfWork.Save();

            return Ok(true);
        }

        [HttpPost("ReactToPost")]
        [Authorize(Roles = "Viewer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReactionDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<ReactionDto>> ReactToPost([FromBody] ReactionDto reactionDto)
        {
            var reaction = await GetCurrentUserReactionFromPost(reactionDto.PostId);

            if (reaction is null)
            {
                string userId = await User.GetCurrentUserId(_userManager);
                reaction = new Reaction
                {
                    PostId = reactionDto.PostId,
                    UserId = userId,
                    ReactionType = (ReactionType)reactionDto.ReactionType,
                    CreationDate=DateTime.Now
                };

                _unitOfWork.Reactions.Insert(reaction);
            }
            else
            {
                reaction.ReactionType = (ReactionType)reactionDto.ReactionType;
                reaction.CreationDate = DateTime.Now;
                _unitOfWork.Reactions.Update(reaction);
            }

            await _unitOfWork.Save();

            return Ok(reactionDto);
        }
    }
}
