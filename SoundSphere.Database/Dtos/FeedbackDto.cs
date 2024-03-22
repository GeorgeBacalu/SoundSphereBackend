using SoundSphere.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos
{
    public class FeedbackDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public FeedbackType Type { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(500, ErrorMessage = "Message can't be longer than 500 characters")]
        public string Message { get; set; } = null!;
        
        public DateTime SentAt { get; set; }
    }
}