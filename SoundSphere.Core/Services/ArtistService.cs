using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;

namespace SoundSphere.Core.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;
        private readonly SoundSphereDbContext _context;
        private readonly IMapper _mapper;

        public ArtistService(IArtistRepository artistRepository, SoundSphereDbContext context, IMapper mapper) => (_artistRepository, _context, _mapper) = (artistRepository, context, mapper);

        public IList<ArtistDto> GetAll(ArtistPaginationRequest? payload)
        {
            IList<ArtistDto> artistDtos = _artistRepository.GetAll(payload).ToDtos(_mapper);
            return artistDtos;
        }

        public ArtistDto GetById(Guid id)
        {
            ArtistDto artistDto = _artistRepository.GetById(id).ToDto(_mapper);
            return artistDto;
        }

        public ArtistDto Add(ArtistDto artistDto)
        {
            Artist artistToCreate = artistDto.ToEntity(_mapper);
            _artistRepository.AddArtistLink(artistToCreate);
            _artistRepository.AddUserArtist(artistToCreate);
            ArtistDto createdArtistDto = _artistRepository.Add(artistToCreate).ToDto(_mapper);
            return createdArtistDto;
        }

        public ArtistDto UpdateById(ArtistDto artistDto, Guid id)
        {
            Artist artistToUpdate = artistDto.ToEntity(_mapper);
            ArtistDto updatedArtistDto = _artistRepository.UpdateById(artistToUpdate, id).ToDto(_mapper);
            return updatedArtistDto;
        }

        public ArtistDto DeleteById(Guid id)
        {
            ArtistDto deletedArtistDto = _artistRepository.DeleteById(id).ToDto(_mapper);
            return deletedArtistDto;
        }

        public IList<ArtistDto> GetRecommendations(int nrRecommendations)
        {
            IList<ArtistDto> recommendationDtos = _context.Artists
                .Include(artist => artist.SimilarArtists)
                .Where(artist => artist.DeletedAt == null)
                .OrderBy(artist => Guid.NewGuid())
                .Take(Math.Max(0, nrRecommendations))
                .ToList()
                .ToDtos(_mapper);
            return recommendationDtos;
        }

        public void ToggleFollow(Guid artistId, Guid userId)
        {
            Artist artist = _artistRepository.GetById(artistId);
            if (artist == null)
                throw new ResourceNotFoundException(string.Format(ArtistNotFound, artistId));
            UserArtist userArtist = _context.UserArtists.First(userArtist => userArtist.ArtistId.Equals(artistId) && userArtist.UserId.Equals(userId));
            userArtist.IsFollowing = !userArtist.IsFollowing;
            _context.SaveChanges();
        }

        public int CountFollowers(Guid id)
        {
            Artist artist = _artistRepository.GetById(id);
            if (artist == null)
                throw new ResourceNotFoundException(string.Format(ArtistNotFound, id));
            int nrFollowers = _context.UserArtists.Count(userArtist => userArtist.ArtistId.Equals(id) && userArtist.IsFollowing);
            return nrFollowers;
        }
    }
}