using System.Text.Json.Serialization;

namespace SoundSphere.Database.Entities
{
    public class Artist
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; } = null!;
        
        public string ImageUrl { get; set; } = null!;
        
        public string ?Bio { get; set; }
        
        [JsonIgnore] public IList<Song>? Songs { get; set; }
        
        public IList<ArtistLink> SimilarArtists { get; set; } = null!;
        
        public bool IsActive { get; set; } = true;

        public override bool Equals(object? obj) => obj is Artist artist &&
            Id.Equals(artist.Id) &&
            Name == artist.Name &&
            ImageUrl == artist.ImageUrl &&
            Bio == artist.Bio &&
            SimilarArtists.SequenceEqual(artist.SimilarArtists) &&
            IsActive == artist.IsActive;

        public override int GetHashCode() => HashCode.Combine(Id, Name, ImageUrl, Bio, Songs, SimilarArtists, IsActive);
    }
}