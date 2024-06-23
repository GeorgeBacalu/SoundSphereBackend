using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos.Common
{
    public record UserPreferencesDto(bool EmailNotifications, Theme Theme);
}