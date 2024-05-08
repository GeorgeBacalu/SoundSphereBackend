using SoundSphere.Database.Dtos.Common;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IAuthorityService
    {
        IList<AuthorityDto> FindAll();

        AuthorityDto FindById(Guid id);

        AuthorityDto Save(AuthorityDto authorityDto);
    }
}