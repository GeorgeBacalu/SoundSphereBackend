using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos
{
    public class FeedbackDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } // ManyToOne with User
        public FeedbackType Type { get; set; }
        public string Message { get; set; } = null!;
        public DateTime SentAt { get; set; }
    }
}
