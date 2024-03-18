using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos
{
    public class SongDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public GenreType Genre { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public int DurationSeconds { get; set; } = 0;
        public Guid AlbumId { get; set; } // ManyToOne with Album
        public IList<Guid> ArtistsIds { get; set; } = new List<Guid>(); // ManyToMany with Artist
        public IList<Guid> SimilarSongsIds { get; set; } = new List<Guid>(); // OneToMany self-referential
        public bool IsActive { get; set; } = true;
    }
}
