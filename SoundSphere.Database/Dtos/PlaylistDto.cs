
namespace SoundSphere.Database.Dtos
{
    public class PlaylistDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public Guid UserId { get; set; } // ManyToOne with User
        public IList<Guid> SongsIds { get; set; } = new List<Guid>(); // ManyToMany with Song
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
