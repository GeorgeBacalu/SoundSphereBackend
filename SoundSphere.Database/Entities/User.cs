using System.Text.Json.Serialization;

namespace SoundSphere.Database.Entities
{
    public class User : BaseEntity
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

        public override bool Equals(object? obj) => obj is User user &&
            Id.Equals(user.Id) &&
            Name.Equals(user.Name) &&
            Email.Equals(user.Email) &&
            Password.Equals(user.Password) &&
            Mobile.Equals(user.Mobile) &&
            Address.Equals(user.Address) &&
            Birthday.Equals(user.Birthday) &&
            Avatar.Equals(user.Avatar) &&
            Role.Equals(user.Role) &&
            Authorities.SequenceEqual(user.Authorities) &&
            CreatedAt.Equals(user.CreatedAt) &&
            UpdatedAt.Equals(user.UpdatedAt) &&
            DeletedAt.Equals(user.DeletedAt);

        public override int GetHashCode() => HashCode.Combine(Id, Name, Email, Password, Mobile, Address, Birthday, HashCode.Combine(Avatar, Role, Authorities, UserSongs, UserArtists, CreatedAt, UpdatedAt, DeletedAt));
    }
}