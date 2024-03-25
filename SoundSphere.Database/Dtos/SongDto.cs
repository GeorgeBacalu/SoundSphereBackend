using SoundSphere.Database.Attributes;
using SoundSphere.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos
{
    public class SongDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(75, ErrorMessage = "Title can't be longer than 75 characters")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "ImageUrl is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string ImageUrl { get; set; } = null!;

        [Required(ErrorMessage = "Genre is required")]
        public GenreType Genre { get; set; }

        [Required(ErrorMessage = "ReleaseDate is required")]
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
            Title == songDto.Title &&
            ImageUrl == songDto.ImageUrl &&
            Genre == songDto.Genre &&
            ReleaseDate.Equals(songDto.ReleaseDate) &&
            DurationSeconds == songDto.DurationSeconds &&
            AlbumId.Equals(songDto.AlbumId) &&
            ArtistsIds.SequenceEqual(songDto.ArtistsIds) &&
            SimilarSongsIds.SequenceEqual(songDto.SimilarSongsIds) &&
            IsActive == songDto.IsActive;

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Title);
            hash.Add(ImageUrl);
            hash.Add(Genre);
            hash.Add(ReleaseDate);
            hash.Add(DurationSeconds);
            hash.Add(AlbumId);
            hash.Add(ArtistsIds);
            hash.Add(SimilarSongsIds);
            hash.Add(IsActive);
            return hash.ToHashCode();
        }
    }
}