namespace SoundSphere.Database.Entities
{
    public class Authority
    {
        public Guid Id { get; set; }
        public AuthorityType Type { get; set; }
        public IList<User> Users { get; set; } = null!; // ManyToMany with User
    }

    public enum AuthorityType { Create, Read, Update, Delete }
}