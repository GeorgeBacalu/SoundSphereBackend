﻿using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Models;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos.Request.Pagination
{
    public record SongPaginationRequest(
        IDictionary<SongSortCriterion, SortOrder>? SortCriteria,
        IList<SongSearchCriterion>? SearchCriteria,
        string? Title,
        GenreType? Genre,
        DateRange? DateRange,
        DurationRange? DurationRange,
        string? AlbumTitle,
        string? ArtistName) : PaginationRequest;

    public enum SongSortCriterion { InvalidSortCriterion, ByTitle = 10, ByReleaseDate = 20 }

    public enum SongSearchCriterion { InvalidSearchCriterion, ByTitle = 10, ByGenre = 20, ByReleaseDateRange = 30, ByDurationSecondsRange = 40, ByAlbumTitle = 50, ByArtistName = 60 }
}