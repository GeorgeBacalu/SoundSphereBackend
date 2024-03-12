using System.Text.Json.Serialization;

namespace SoundSphere.Database.Entities
{
    public class Song
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public GenreType Genre { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public int DurationSeconds { get; set; } = 0;
        public Album Album { get; set; } = null!; // ManyToOne with Album
        public IList<Artist> Artists { get; set; } = null!; // ManyToMany with Artist
        [JsonIgnore] public IList<Playlist>? Playlists { get; set; } // ManyToMany with Playlist
        public IList<SongLink> SimilarSongs { get; set; } = null!; // OneToMany self-referential
        public bool IsActive { get; set; } = true;
    }

    public enum GenreType { Pop, Rock, Rnb, HipHop, Dance, Techno, Latino, Hindi, Reggae, Jazz, Classical, Country }
}