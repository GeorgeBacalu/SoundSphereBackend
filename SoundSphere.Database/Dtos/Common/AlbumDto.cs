using SoundSphere.Database.Attributes;
using SoundSphere.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos.Common
{
    public class AlbumDto : BaseEntity
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

        public override bool Equals(object? obj) => obj is AlbumDto albumDto &&
            Id.Equals(albumDto.Id) &&
            Title.Equals(albumDto.Title) &&
            ImageUrl.Equals(albumDto.ImageUrl) &&
            ReleaseDate.Equals(albumDto.ReleaseDate) &&
            SimilarAlbumsIds.SequenceEqual(albumDto.SimilarAlbumsIds) &&
            CreatedAt.Equals(albumDto.CreatedAt) &&
            UpdatedAt.Equals(albumDto.UpdatedAt) &&
            DeletedAt.Equals(albumDto.DeletedAt);

        public override int GetHashCode() => HashCode.Combine(Id, Title, ImageUrl, ReleaseDate, SimilarAlbumsIds, HashCode.Combine(CreatedAt, UpdatedAt, DeletedAt));
    }
}