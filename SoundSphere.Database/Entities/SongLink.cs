namespace SoundSphere.Database.Entities
{
    public class SongLink
    {
        public Guid SongId { get; set; }
        public Guid SimilarSongId { get; set; }
        public Song? Song { get; set; }
        public Song? SimilarSong { get; set; }
    }
}