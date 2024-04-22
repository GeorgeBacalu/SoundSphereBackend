
namespace SoundSphere.Database.Entities
{
    public class UserSong
    {
        public Guid UserId { get; set; }
        
        public Guid SongId { get; set; }
        
        public User? User { get; set; }
        
        public Song? Song { get; set; }
        
        public int PlayCount { get; set; } = 0;

        public override bool Equals(object? obj) => obj is UserSong userSong && UserId.Equals(userSong.UserId) && SongId.Equals(userSong.SongId) && PlayCount == userSong.PlayCount;

        public override int GetHashCode() => HashCode.Combine(UserId, SongId, User, Song, PlayCount);
    }
}