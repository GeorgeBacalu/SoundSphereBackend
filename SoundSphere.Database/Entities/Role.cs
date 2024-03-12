namespace SoundSphere.Database.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        public RoleType Type { get; set; }
    }

    public enum RoleType { Administrator, Moderator, Listener }
}