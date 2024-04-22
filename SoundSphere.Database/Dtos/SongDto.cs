using SoundSphere.Database.Attributes;
using SoundSphere.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos
{
    public class SongDto
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(75, ErrorMessage = "Title can't be longer than 75 characters")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "ImageUrl is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string ImageUrl { get; set; } = null!;

        [Required(ErrorMessage = "Genre is required")]
        public GenreType Genre { get; set; }

        [Required(ErrorMessage = "Release date is required")]
        [Date(ErrorMessage = "Release date can't be in the future")]
        public DateOnly ReleaseDate { get; set; }

        [Range(1, 300, ErrorMessage = "Song can't be longer than 5 minutes")]
        public int DurationSeconds { get; set; } = 0;

        [Required(ErrorMessage = "AlbumId is required")]
        public Guid AlbumId { get; set; }

        [MaxLength(15, ErrorMessage = "There can't be more than 15 artists")]
        public IList<Guid> ArtistsIds { get; set; } = new List<Guid>();

        [MaxLength(15, ErrorMessage = "There can't be more than 15 similar songs")]
        public IList<Guid> SimilarSongsIds { get; set; } = new List<Guid>();
        
        public bool IsActive { get; set; } = true;

        public override bool Equals(object? obj) => obj is SongDto songDto &&
            Id.Equals(songDto.Id) &&
            Title.Equals(songDto.Title) &&
            ImageUrl.Equals(songDto.ImageUrl) &&
            Genre == songDto.Genre &&
            ReleaseDate.Equals(songDto.ReleaseDate) &&
            DurationSeconds == songDto.DurationSeconds &&
            AlbumId.Equals(songDto.AlbumId) &&
            ArtistsIds.SequenceEqual(songDto.ArtistsIds) &&
            SimilarSongsIds.SequenceEqual(songDto.SimilarSongsIds) &&
            IsActive == songDto.IsActive;

        public override int GetHashCode() => HashCode.Combine(Id, Title, ImageUrl, Genre, ReleaseDate, DurationSeconds, AlbumId, HashCode.Combine(ArtistsIds, SimilarSongsIds, IsActive));
    }
}