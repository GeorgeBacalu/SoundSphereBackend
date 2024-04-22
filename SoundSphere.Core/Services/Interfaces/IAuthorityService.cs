using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IAuthorityService
    {
        IList<AuthorityDto> FindAll();

        AuthorityDto FindById(Guid id);

        AuthorityDto Save(AuthorityDto authorityDto);

        IList<AuthorityDto> ConvertToDtos(IList<Authority> authorities);

        IList<Authority> ConvertToEntities(IList<AuthorityDto> authorityDtos);

        AuthorityDto ConvertToDto(Authority authority);

        Authority ConvertToEntity(AuthorityDto authorityDto);
    }
}