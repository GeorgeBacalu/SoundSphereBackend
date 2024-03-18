

namespace SoundSphere.Database.Dtos
{
    public class AlbumDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public DateOnly ReleaseDate { get; set; }

        public IList<Guid> SimilarAlbumsIds { get; set; } = new List<Guid>();

        public bool IsActive { get; set; } = true;
    }
}
