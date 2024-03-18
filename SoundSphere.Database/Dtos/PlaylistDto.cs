namespace SoundSphere.Database.Dtos
{
    public class PlaylistDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public Guid UserId { get; set; }
        public IList<Guid> SongsIds { get; set; } = new List<Guid>();
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}