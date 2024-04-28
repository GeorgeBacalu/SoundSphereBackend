using AutoMapper;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Album, AlbumDto>();
            CreateMap<Artist, ArtistDto>();
            CreateMap<Authority, AuthorityDto>();
            CreateMap<Feedback, FeedbackDto>();
            CreateMap<Notification, NotificationDto>();
            CreateMap<Playlist, PlaylistDto>();
            CreateMap<Role, RoleDto>();
            CreateMap<Song, SongDto>();
            CreateMap<User, UserDto>();

            CreateMap<AlbumDto, Album>();
            CreateMap<ArtistDto, Artist>();
            CreateMap<AuthorityDto, Authority>();
            CreateMap<FeedbackDto, Feedback>();
            CreateMap<NotificationDto, Notification>();
            CreateMap<PlaylistDto, Playlist>();
            CreateMap<RoleDto, Role>();
            CreateMap<SongDto, Song>();
            CreateMap<UserDto, User>();
        }
    }
}