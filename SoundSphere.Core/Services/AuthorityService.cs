using AutoMapper;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class AuthorityService : IAuthorityService
    {
        private readonly IAuthorityRepository _authorityRepository;
        private readonly IMapper _mapper;

        public AuthorityService(IAuthorityRepository authorityRepository, IMapper mapper) => (_authorityRepository, _mapper) = (authorityRepository, mapper);

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

        public AuthorityDto ConvertToDto(Authority authority) => _mapper.Map<AuthorityDto>(authority);

        public Authority ConvertToEntity(AuthorityDto authorityDto) => _mapper.Map<Authority>(authorityDto);
    }
}