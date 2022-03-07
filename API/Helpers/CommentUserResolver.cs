using API.Dtos;
using API.Extensions;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace API.Helpers
{
    public class CommentUserResolver : IValueResolver<Comment, ConsultCommentDto, bool>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        public CommentUserResolver(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool Resolve(Comment source, ConsultCommentDto destination, bool destMember, ResolutionContext context)
        {
            string userId = _httpContextAccessor.HttpContext.User.GetCurrentUserId(_userManager).Result;

            return source.UserId == userId;
        }
    }
}
