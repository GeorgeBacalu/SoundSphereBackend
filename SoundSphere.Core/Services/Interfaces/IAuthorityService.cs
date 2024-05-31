using SoundSphere.Database.Dtos.Common;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IAuthorityService
    {
        IList<AuthorityDto> GetAll();

        AuthorityDto GetById(Guid id);

        AuthorityDto Add(AuthorityDto authorityDto);
    }
}