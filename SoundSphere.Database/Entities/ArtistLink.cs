namespace SoundSphere.Database.Entities
{
    public class ArtistLink
    {
        public Guid ArtistId { get; set; }

        public Guid SimilarArtistId { get; set; }

        public Artist? Artist { get; set; }

        public Artist? SimilarArtist { get; set; }

        public override bool Equals(object? obj) => obj is ArtistLink artistLink && ArtistId.Equals(artistLink.ArtistId) && SimilarArtistId.Equals(artistLink.SimilarArtistId);

        public override int GetHashCode() => HashCode.Combine(ArtistId, SimilarArtistId, Artist, SimilarArtist);
    }
}