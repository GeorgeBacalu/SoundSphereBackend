namespace SoundSphere.Database.Entities
{
    public class UserArtist
    {
        public Guid UserId { get; set; }
        public Guid ArtistId { get; set; }
        public User? User { get; set; }
        public Artist? Artist { get; set; }
        public bool IsFollowing { get; set; } = false;
    }
}