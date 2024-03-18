using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class AuthorityService : IAuthorityService
    {
        private readonly IAuthorityRepository _authorityRepository;

        public AuthorityService(IAuthorityRepository authorityRepository) => _authorityRepository = authorityRepository;

        public IList<AuthorityDto> FindAll() => ConvertToDtos(_authorityRepository.FindAll());

        public AuthorityDto FindById(Guid id) => ConvertToDto(_authorityRepository.FindById(id));

        public AuthorityDto Save(AuthorityDto authorityDto)
        {
            Authority authority = ConvertToEntity(authorityDto);
            if (authority.Id == Guid.Empty) authority.Id = Guid.NewGuid();
            return ConvertToDto(_authorityRepository.Save(authority));
        }

        public IList<AuthorityDto> ConvertToDtos(IList<Authority> authorities) => authorities.Select(ConvertToDto).ToList();

        public IList<Authority> ConvertToEntities(IList<AuthorityDto> authorityDtos) => authorityDtos.Select(ConvertToEntity).ToList();

        public AuthorityDto ConvertToDto(Authority authority) => new AuthorityDto
        {
            Id = authority.Id,
            Type = authority.Type
        };

        public Authority ConvertToEntity(AuthorityDto authorityDto) => new Authority
        {
            Id = authorityDto.Id,
            Type = authorityDto.Type
        };
    }
}