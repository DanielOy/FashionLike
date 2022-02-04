using API.Dtos;
using AutoMapper;
using Core.Entities;
using System.Linq;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Post, PostDto>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.UserName))
                .ForMember(d => d.Tags, o => o.MapFrom(s => s.Tags.Select(x => x.Name)))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<PostUrlResolver>());
            CreateMap<Tag, TagDto>();
        }
    }
}
