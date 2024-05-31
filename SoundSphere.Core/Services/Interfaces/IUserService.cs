using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IUserService
    {
        IList<UserDto> GetAll();

        IList<UserDto> GetAllActive();

        IList<UserDto> GetAllPagination(UserPaginationRequest payload);

        IList<UserDto> GetAllActivePagination(UserPaginationRequest payload);

        UserDto GetById(Guid id);

        UserDto Add(UserDto userDto);

        UserDto UpdateById(UserDto userDto, Guid id);

        UserDto DeleteById(Guid id);
    }
}