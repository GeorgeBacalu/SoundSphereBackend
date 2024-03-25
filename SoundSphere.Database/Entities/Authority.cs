using System.Text.Json.Serialization;

namespace SoundSphere.Database.Entities
{
    public class Authority
    {
        public Guid Id { get; set; }

        public AuthorityType Type { get; set; }

        [JsonIgnore] public IList<User>? Users { get; set; }

        public override bool Equals(object? obj) => obj is Authority authority && Id.Equals(authority.Id) && Type == authority.Type;

        public override int GetHashCode() => HashCode.Combine(Id, Type, Users);
    }

    public enum AuthorityType { Create, Read, Update, Delete }
}