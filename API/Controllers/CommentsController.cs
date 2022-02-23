using API.Dtos;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public CommentsController(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("{PostId}")]
        [Authorize(Roles = "Viewer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ConsultCommentDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<IEnumerable<ConsultCommentDto>>> Get(int PostId)
        {
            var comments = await _unitOfWork.Comments.GetAllByExpression(x => x.PostId == PostId, null, nameof(Comment.User));

            var commentDtos = _mapper.Map<IEnumerable<Comment>, IEnumerable<ConsultCommentDto>>(comments);

            return Ok(commentDtos);
        }

        [HttpPost]
        [Authorize(Roles = "Viewer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<CommentDto>> Post([FromBody] CommentDto commentDto)
        {
            string userId = await User.GetCurrentUserId(_userManager);

            var comment = new Comment
            {
                PostId = commentDto.PostId,
                Text = commentDto.Text,
                UserId = userId,
                CreationDate = DateTime.Now
            };

            _unitOfWork.Comments.Insert(comment);
            await _unitOfWork.Save();

            return Ok(commentDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Viewer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<CommentDto>> Put(int id, [FromBody] CommentDto commentDto)
        {
            var comment = await _unitOfWork.Comments.GetByID(id);

            comment.Text = commentDto.Text;

            _unitOfWork.Comments.Update(comment);
            await _unitOfWork.Save();

            return Ok(commentDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Viewer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            _unitOfWork.Comments.DeleteByID(id);
            await _unitOfWork.Save();

            return Ok(true);
        }
    }
}
