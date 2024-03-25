namespace SoundSphere.Database.Entities
{
    public class SongLink
    {
        public Guid SongId { get; set; }

        public Guid SimilarSongId { get; set; }

        public Song? Song { get; set; }

        public Song? SimilarSong { get; set; }

        public override bool Equals(object? obj) => obj is SongLink songLink && SongId.Equals(songLink.SongId) && SimilarSongId.Equals(songLink.SimilarSongId);

        public override int GetHashCode() => HashCode.Combine(SongId, SimilarSongId, Song, SimilarSong);
    }
}