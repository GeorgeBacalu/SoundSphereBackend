using SoundSphere.Database.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos.Common
{
    public class AlbumDto
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(75, ErrorMessage = "Title can't be longer than 75 characters")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Image URL is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string ImageUrl { get; set; } = null!;

        [Required(ErrorMessage = "Release date is required")]
        [Date(ErrorMessage = "Release date can't be in the future")]
        public DateOnly ReleaseDate { get; set; }

        [MaxLength(15, ErrorMessage = "There can't be more than 15 similar albums")]
        public IList<Guid> SimilarAlbumsIds { get; set; } = new List<Guid>();

        public bool IsActive { get; set; } = true;

        public override bool Equals(object? obj) => obj is AlbumDto albumDto &&
            Id.Equals(albumDto.Id) &&
            Title.Equals(albumDto.Title) &&
            ImageUrl.Equals(albumDto.ImageUrl) &&
            ReleaseDate.Equals(albumDto.ReleaseDate) &&
            SimilarAlbumsIds.SequenceEqual(albumDto.SimilarAlbumsIds) &&
            IsActive == albumDto.IsActive;

        public override int GetHashCode() => HashCode.Combine(Id, Title, ImageUrl, ReleaseDate, SimilarAlbumsIds, IsActive);
    }
}