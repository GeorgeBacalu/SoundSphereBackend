using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        [JsonIgnore] public IList<UserSong>? UserSongs { get; set; } // ManyToMany with Song
        [JsonIgnore] public IList<UserArtist>? UserArtists { get; set; } // ManyToMany with Artist
        public bool IsActive { get; set; } = true;
    }
}