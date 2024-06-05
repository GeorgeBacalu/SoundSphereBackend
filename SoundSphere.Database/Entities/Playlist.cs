namespace SoundSphere.Database.Entities
{
    public class Playlist : BaseEntity
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; } = null!;
        
        public User User { get; set; } = null!;
        
        public IList<Song> Songs { get; set; } = new List<Song>();

        public override bool Equals(object? obj) => obj is Playlist playlist &&
            Id.Equals(playlist.Id) &&
            Title.Equals(playlist.Title) &&
            User.Equals(playlist.User) &&
            Songs.SequenceEqual(playlist.Songs) &&
            CreatedAt.Equals(playlist.CreatedAt) &&
            UpdatedAt.Equals(playlist.UpdatedAt) &&
            DeletedAt.Equals(playlist.DeletedAt);

        public override int GetHashCode() => HashCode.Combine(Id, Title, User, Songs, HashCode.Combine(CreatedAt, UpdatedAt, DeletedAt));
    }
}