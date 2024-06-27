using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IArtistService
    {
        IList<ArtistDto> GetAll(ArtistPaginationRequest? payload);

        ArtistDto GetById(Guid id);

        ArtistDto Add(ArtistDto artistDto);

        ArtistDto UpdateById(ArtistDto artistDto, Guid id);

        ArtistDto DeleteById(Guid id);

        IList<ArtistDto> GetRecommendations(int nrRecommendations);

        void ToggleFollow(Guid artistId, Guid userId);

        int CountFollowers(Guid id);
    }
}