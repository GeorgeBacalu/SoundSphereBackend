using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Dtos.Request.Auth;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IUserService
    {
        IList<UserDto> GetAll(UserPaginationRequest? payload);

        UserDto GetById(Guid id);

        UserDto? Register(RegisterRequest payLoad);

        string? Login(LoginRequest payLoad);

        UserDto UpdateById(UserDto userDto, Guid id);

        UserDto DeleteById(Guid id);

        UserDto? UpdatePreferences(UserPreferencesDto payload, Guid userId);

        void ChangePassword(ChangePasswordRequest payLoad, Guid userId);
    }
}