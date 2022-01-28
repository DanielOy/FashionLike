using API.Dtos;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        [Authorize(Roles = "Administrator,Viewer")]
        [HttpGet]
        public async Task<ActionResult<Pagination<PostDto>>> Get([FromQuery] PostSpecParams postParams)
        {
            var spec = new PostSpecification(postParams);

            var countSpec = new PostCountSpecification(postParams);

            var total = await _unitOfWork.Posts.CountAsync(countSpec);

            var posts = await _unitOfWork.Posts.GetAllBySpecification(spec);

            var data = _mapper.Map<IEnumerable<Post>, IEnumerable<PostDto>>(posts);

            var page = new Pagination<PostDto>(postParams.PageIndex, postParams.PageSize, total, data);

            return Ok(page);
        }

        [Authorize(Roles = "Administrator,Viewer")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> Get(int id)
        {
            var post = await _unitOfWork.Posts.GetByID(id);

            var postDto = _mapper.Map<Post, PostDto>(post);

            return Ok(postDto);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult<PostDto>> Post([FromBody] PostToCreateDto postToCreateDto)
        {
            string userId = await User.GetCurrentUserId(_userManager);

            string normalizedName = _imagesService.GetNormalizedName(postToCreateDto.FileName);

            await _imagesService.StorageImage(postToCreateDto.ImageBase64, normalizedName, _environmet.ContentRootPath);

            var post = new Post
            {
                Active = true,
                CreationDate = DateTime.Now,
                Description = postToCreateDto.Description,
                UserId = userId,
                PictureName = normalizedName
            };

            _unitOfWork.Posts.Insert(post);
            await _unitOfWork.Save();

            var postDto = _mapper.Map<Post, PostDto>(post);

            return Ok(postDto);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
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

            _unitOfWork.Posts.Update(post);
            await _unitOfWork.Save();

            var updatedPost = _mapper.Map<Post, PostDto>(post);

            return Ok(updatedPost);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            _unitOfWork.Posts.DeleteByID(id);

            await _unitOfWork.Save();

            return Ok(true);
        }
    }
}
