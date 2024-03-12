using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        [EmailAddress] public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Mobile { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateOnly Birthday { get; set; }
        public string Avatar { get; set; } = null!;
        public Role Role { get; set; } = null!; // ManyToOne with Role
        public IList<Authority> Authorities { get; set; } = null!; // ManyToMany with Authority
        public IList<UserSong> UserSongs { get; set; } = null!; // ManyToMany with Song
        public IList<UserArtist> UserArtists { get; set; } = null!; // ManyToMany with Artist
        public bool IsActive { get; set; } = true;
    }
}