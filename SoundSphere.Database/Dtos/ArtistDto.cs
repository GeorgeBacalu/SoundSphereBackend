namespace SoundSphere.Database.Dtos
{
    public class ArtistDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Bio { get; set; } = null!;
        // public IList<Guid> SongsIds { get; set; } = new List<Guid>();
        public IList<Guid> SimilarArtistsIds { get; set; } = new List<Guid>();
        public bool IsActive { get; set; } = true;
    }
}