using API.Dtos;
using API.Extensions;
using AutoMapper;
using Core.Entities;
using Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class PostUserResolver : IValueResolver<Post, PostDto, ReactionType>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        public PostUserResolver(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public ReactionType Resolve(Post source, PostDto destination, ReactionType destMember, ResolutionContext context)
        {
            string userId = _httpContextAccessor.HttpContext.User.GetCurrentUserId(_userManager).Result;

            return source.Reactions.FirstOrDefault(x => x.UserId == userId)?.ReactionType ?? ReactionType.None;
        }
    }
}
