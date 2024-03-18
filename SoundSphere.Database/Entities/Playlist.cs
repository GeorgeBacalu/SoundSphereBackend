namespace SoundSphere.Database.Entities
{
    public class Playlist
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public User User { get; set; } = null!;
        public IList<Song> Songs { get; set; } = new List<Song>();
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}