namespace SoundSphere.Database.Entities
{
    public class UserSong
    {
        public Guid UserId { get; set; }
        public Guid SongId { get; set; }
        public User? User { get; set; }
        public Song? Song { get; set; }
        public int PlayCount { get; set; } = 0;
    }
}