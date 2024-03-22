﻿using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos
{
    public class PlaylistDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(75, ErrorMessage = "Title can't be longer than 75 characters")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; }

        [MaxLength(100, ErrorMessage = "There can't be more than 100 songs in a playlist")]
        public IList<Guid> SongsIds { get; set; } = new List<Guid>();
        
        public DateTime CreatedAt { get; set; }
        
        public bool IsActive { get; set; } = true;
    }
}