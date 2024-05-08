using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IUserService
    {
        IList<UserDto> FindAll();

        IList<UserDto> FindAllActive();

        IList<UserDto> FindAllPagination(UserPaginationRequest payload);

        IList<UserDto> FindAllActivePagination(UserPaginationRequest payload);

        UserDto FindById(Guid id);

        UserDto Save(UserDto userDto);

        UserDto UpdateById(UserDto userDto, Guid id);

        UserDto DisableById(Guid id);
    }
}