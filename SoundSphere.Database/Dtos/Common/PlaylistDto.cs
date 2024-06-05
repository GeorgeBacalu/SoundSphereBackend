using SoundSphere.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos.Common
{
    public class PlaylistDto : BaseEntity
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(75, ErrorMessage = "Title can't be longer than 75 characters")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; }

        [MaxLength(100, ErrorMessage = "There can't be more than 100 songs in a playlist")]
        public IList<Guid> SongsIds { get; set; } = new List<Guid>();

        public override bool Equals(object? obj) => obj is PlaylistDto playlistDto &&
            Id.Equals(playlistDto.Id) &&
            Title.Equals(playlistDto.Title) &&
            UserId.Equals(playlistDto.UserId) &&
            SongsIds.SequenceEqual(playlistDto.SongsIds) &&
            CreatedAt.Equals(playlistDto.CreatedAt) &&
            UpdatedAt.Equals(playlistDto.UpdatedAt) &&
            DeletedAt.Equals(playlistDto.DeletedAt);

        public override int GetHashCode() => HashCode.Combine(Id, Title, UserId, SongsIds, HashCode.Combine(CreatedAt, UpdatedAt, DeletedAt));
    }
}