namespace SoundSphere.Database.Entities
{
    public class AlbumLink
    {
        public Guid AlbumId { get; set; }
        
        public Guid SimilarAlbumId { get; set; }
        
        public Album? Album { get; set; }
        
        public Album? SimilarAlbum { get; set; }

        public override bool Equals(object? obj) => obj is AlbumLink albumLink && AlbumId.Equals(albumLink.AlbumId) && SimilarAlbumId.Equals(albumLink.SimilarAlbumId);

        public override int GetHashCode() => HashCode.Combine(AlbumId, SimilarAlbumId, Album, SimilarAlbum);
    }
}