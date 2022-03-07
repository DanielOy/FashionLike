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
                .ForMember(destiny => destiny.UserName, origin => origin.MapFrom(s => s.User.UserName))
                .ForMember(destiny => destiny.Tags, origin => origin.MapFrom(s => s.Tags.Select(x => x.Name)))
                .ForMember(destiny => destiny.PictureUrl, origin => origin.MapFrom<PostUrlResolver>())
                .ForMember(destiny => destiny.LikeCount, origin => origin.MapFrom(s => s.Reactions.Count(x => x.ReactionType == Core.Enums.ReactionType.Like)))
                .ForMember(destiny => destiny.DislikeCount, origin => origin.MapFrom(s => s.Reactions.Count(x => x.ReactionType == Core.Enums.ReactionType.Dislike)))
                .ForMember(destiny=>destiny.UserReaction, origin=>origin.MapFrom<PostUserResolver>());
            CreateMap<Tag, TagDto>();
            CreateMap<Comment, ConsultCommentDto>()
                .ForMember(destiny => destiny.User, origin => origin.MapFrom(source => source.User.DisplayName));
        }
    }
}
