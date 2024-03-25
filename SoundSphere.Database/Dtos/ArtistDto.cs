using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos
{
    public class ArtistDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(75, ErrorMessage = "Name can't be longer than 75 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Image URL is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string ImageUrl { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Bio can't be longer than 500 characters")]
        public string ?Bio { get; set; }

        [MaxLength(15, ErrorMessage = "There can't be more than 15 similar artists")]
        public IList<Guid> SimilarArtistsIds { get; set; } = new List<Guid>();
        
        public bool IsActive { get; set; } = true;

        public override bool Equals(object? obj) => obj is ArtistDto artistDto &&
            Id.Equals(artistDto.Id) &&
            Name == artistDto.Name &&
            ImageUrl == artistDto.ImageUrl &&
            Bio == artistDto.Bio &&
            SimilarArtistsIds.SequenceEqual(artistDto.SimilarArtistsIds) &&
            IsActive == artistDto.IsActive;

        public override int GetHashCode() => HashCode.Combine(Id, Name, ImageUrl, Bio, SimilarArtistsIds, IsActive);
    }
}