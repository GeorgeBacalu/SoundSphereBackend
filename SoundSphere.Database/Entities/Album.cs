namespace SoundSphere.Database.Entities
{
    public class Album
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public DateOnly ReleaseDate { get; set; }
        public IList<AlbumLink> SimilarAlbums { get; set; } = null!; // OneToMany self-referential
        public bool IsActive { get; set; } = true;
    }
}