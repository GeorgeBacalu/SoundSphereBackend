
namespace SoundSphere.Database.Entities
{
    public class UserArtist
    {
        public Guid UserId { get; set; }
        
        public Guid ArtistId { get; set; }
        
        public User? User { get; set; }
        
        public Artist? Artist { get; set; }
        
        public bool IsFollowing { get; set; } = false;

        public override bool Equals(object? obj) => obj is UserArtist userArtist && UserId.Equals(userArtist.UserId) && ArtistId.Equals(userArtist.ArtistId) && IsFollowing == userArtist.IsFollowing;

        public override int GetHashCode() => HashCode.Combine(UserId, ArtistId, User, Artist, IsFollowing);
    }
}