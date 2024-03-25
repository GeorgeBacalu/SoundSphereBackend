namespace SoundSphere.Database.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        
        public RoleType Type { get; set; }

        public override bool Equals(object? obj) => obj is Role role && Id.Equals(role.Id) && Type == role.Type;

        public override int GetHashCode() => HashCode.Combine(Id, Type);
    }

    public enum RoleType { Administrator, Moderator, Listener }
}