﻿using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IUserService
    {
        IList<UserDto> GetAll(UserPaginationRequest payload);

        UserDto GetById(Guid id);

        UserDto? Register(RegisterRequest payLoad);

        string? Login(LoginRequest payLoad);

        UserDto UpdateById(UserDto userDto, Guid id);

        UserDto DeleteById(Guid id);
    }
}