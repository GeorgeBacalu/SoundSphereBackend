namespace SoundSphere.Database.Entities
{
    public class Album
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; } = null!;
        
        public string ImageUrl { get; set; } = null!;
        
        public DateOnly ReleaseDate { get; set; }
        
        public IList<AlbumLink> SimilarAlbums { get; set; } = null!;
        
        public bool IsActive { get; set; } = true;

        public override bool Equals(object? obj) => obj is Album album &&
            Id.Equals(album.Id) &&
            Title.Equals(album.Title) &&
            ImageUrl.Equals(album.ImageUrl) &&
            ReleaseDate.Equals(album.ReleaseDate) &&
            SimilarAlbums.SequenceEqual(album.SimilarAlbums) &&
            IsActive == album.IsActive;

        public override int GetHashCode() => HashCode.Combine(Id, Title, ImageUrl, ReleaseDate, SimilarAlbums, IsActive);
    }
}