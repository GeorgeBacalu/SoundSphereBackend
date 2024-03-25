using System.Text.Json.Serialization;

namespace SoundSphere.Database.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; } = null!;
        
        public string Email { get; set; } = null!;
        
        public string Password { get; set; } = null!;
        
        public string Mobile { get; set; } = null!;
        
        public string Address { get; set; } = null!;
        
        public DateOnly Birthday { get; set; }
        
        public string Avatar { get; set; } = null!;
        
        public Role Role { get; set; } = null!;
        
        public IList<Authority> Authorities { get; set; } = null!;
        
        [JsonIgnore] public IList<UserSong>? UserSongs { get; set; }
        
        [JsonIgnore] public IList<UserArtist>? UserArtists { get; set; }
        
        public bool IsActive { get; set; } = true;

        public override bool Equals(object? obj) => obj is User user &&
            Id.Equals(user.Id) &&
            Name == user.Name &&
            Email == user.Email &&
            Password == user.Password &&
            Mobile == user.Mobile &&
            Address == user.Address &&
            Birthday.Equals(user.Birthday) &&
            Avatar == user.Avatar &&
            Role.Equals(user.Role) &&
            Authorities.SequenceEqual(user.Authorities) &&
            IsActive == user.IsActive;

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Name);
            hash.Add(Email);
            hash.Add(Password);
            hash.Add(Mobile);
            hash.Add(Address);
            hash.Add(Birthday);
            hash.Add(Avatar);
            hash.Add(Role);
            hash.Add(Authorities);
            hash.Add(UserSongs);
            hash.Add(UserArtists);
            hash.Add(IsActive);
            return hash.ToHashCode();
        }
    }
}