using API.Dtos;
using AutoMapper;
using Core.Entities;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public class PostUrlResolver : IValueResolver<Post, PostDto, string>
    {
        private readonly IConfiguration _configuration;

        public PostUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Post source, PostDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureName))
            {
                return $"{_configuration["ApiUrl"]}{ImagesService.PostImageFolder}/{source.PictureName}";
            }

            return null;
        }
    }
}
