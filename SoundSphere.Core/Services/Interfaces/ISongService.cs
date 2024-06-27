using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Dtos.Response;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface ISongService
    {
        IList<SongDto> GetAll(SongPaginationRequest? payload);

        SongDto GetById(Guid id);

        SongDto Add(SongDto songDto);

        SongDto UpdateById(SongDto songDto, Guid id);

        SongDto DeleteById(Guid id);

        IList<SongDto> GetRecommendations(int nrRecommendations);

        SongStatisticsDto GetStatistics(DateTime? startDate, DateTime? endDate);

        void Play(Guid songId, Guid userId);

        int GetPlayCount(Guid songId, Guid userId);
    }
}