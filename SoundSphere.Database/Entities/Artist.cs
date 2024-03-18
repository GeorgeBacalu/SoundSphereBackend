using System.Text.Json.Serialization;

namespace SoundSphere.Database.Entities
{
    public class Artist
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Bio { get; set; } = null!;
        [JsonIgnore] public IList<Song>? Songs { get; set; }
        public IList<ArtistLink> SimilarArtists { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }
}