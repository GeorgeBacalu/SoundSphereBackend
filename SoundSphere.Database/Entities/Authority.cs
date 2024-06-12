using System.Text.Json.Serialization;

namespace SoundSphere.Database.Entities
{
    public class Authority
    {
        public Guid Id { get; set; }

        public AuthorityType Type { get; set; }

        [JsonIgnore] public IList<User>? Users { get; set; }

        public DateTime CreatedAt { get; set; }

        public override bool Equals(object? obj) => obj is Authority authority && Id.Equals(authority.Id) && Type == authority.Type && CreatedAt == authority.CreatedAt;

        public override int GetHashCode() => HashCode.Combine(Id, Type, Users, CreatedAt);
    }

    public enum AuthorityType { InvalidAuthorityType, Create = 10, Read = 20, Update = 30, Delete = 40 }
}