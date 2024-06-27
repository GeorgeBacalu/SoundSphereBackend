namespace SoundSphere.Database.Entities
{
    public class Album : BaseEntity
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; } = null!;
        
        public string ImageUrl { get; set; } = null!;
        
        public DateOnly ReleaseDate { get; set; }
        
        public IList<AlbumLink> SimilarAlbums { get; set; } = new List<AlbumLink>();

        public override bool Equals(object? obj) => obj is Album album &&
            Id.Equals(album.Id) &&
            Title.Equals(album.Title) &&
            ImageUrl.Equals(album.ImageUrl) &&
            ReleaseDate.Equals(album.ReleaseDate) &&
            (SimilarAlbums?.SequenceEqual(album.SimilarAlbums ?? new List<AlbumLink>()) ?? album.SimilarAlbums == null) &&
            CreatedAt.Equals(album.CreatedAt) &&
            UpdatedAt.Equals(album.UpdatedAt) &&
            DeletedAt.Equals(album.DeletedAt);

        public override int GetHashCode() => HashCode.Combine(Id, Title, ImageUrl, ReleaseDate, SimilarAlbums, HashCode.Combine(CreatedAt, UpdatedAt, DeletedAt));
    }
}