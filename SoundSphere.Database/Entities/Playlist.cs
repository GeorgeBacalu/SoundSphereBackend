﻿namespace SoundSphere.Database.Entities
{
    public class Playlist
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; } = null!;
        
        public User User { get; set; } = null!;
        
        public IList<Song> Songs { get; set; } = new List<Song>();
        
        public DateTime CreatedAt { get; set; }
        
        public bool IsActive { get; set; } = true;

        public override bool Equals(object? obj) => obj is Playlist playlist &&
            Id.Equals(playlist.Id) &&
            Title == playlist.Title &&
            User.Equals(playlist.User) &&
            Songs.SequenceEqual(playlist.Songs) &&
            CreatedAt == playlist.CreatedAt &&
            IsActive == playlist.IsActive;

        public override int GetHashCode() => HashCode.Combine(Id, Title, User, Songs, CreatedAt, IsActive);
    }
}