using AutoMapper;
using FluentAssertions;
using Moq;
using SoundSphere.Core.Services;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Constants;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Tests.Mocks;

namespace SoundSphere.Tests.Unit.Services
{
    public class AuthorityServiceTest
    {
        private readonly Mock<IAuthorityRepository> _authorityRepository = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly IAuthorityService _authorityService;

        private readonly Authority _authority1 = AuthorityMock.GetMockedAuthority1();
        private readonly Authority _authority2 = AuthorityMock.GetMockedAuthority2();
        private readonly Authority _authority3 = AuthorityMock.GetMockedAuthority3();
        private readonly Authority _authority4 = AuthorityMock.GetMockedAuthority4();
        private readonly IList<Authority> _authorities = AuthorityMock.GetMockedAuthorities();
        private readonly AuthorityDto _authorityDto1 = AuthorityMock.GetMockedAuthorityDto1();
        private readonly AuthorityDto _authorityDto2 = AuthorityMock.GetMockedAuthorityDto2();
        private readonly AuthorityDto _authorityDto3 = AuthorityMock.GetMockedAuthorityDto3();
        private readonly AuthorityDto _authorityDto4 = AuthorityMock.GetMockedAuthorityDto4();
        private readonly IList<AuthorityDto> _authorityDtos = AuthorityMock.GetMockedAuthorityDtos();

        public AuthorityServiceTest()
        {
            _mapper.Setup(mock => mock.Map<AuthorityDto>(_authority1)).Returns(_authorityDto1);
            _mapper.Setup(mock => mock.Map<AuthorityDto>(_authority2)).Returns(_authorityDto2);
            _mapper.Setup(mock => mock.Map<AuthorityDto>(_authority3)).Returns(_authorityDto3);
            _mapper.Setup(mock => mock.Map<AuthorityDto>(_authority4)).Returns(_authorityDto4);
            _mapper.Setup(mock => mock.Map<Authority>(_authorityDto1)).Returns(_authority1);
            _mapper.Setup(mock => mock.Map<Authority>(_authorityDto2)).Returns(_authority2);
            _mapper.Setup(mock => mock.Map<Authority>(_authorityDto3)).Returns(_authority3);
            _mapper.Setup(mock => mock.Map<Authority>(_authorityDto4)).Returns(_authority4);
            _authorityService = new AuthorityService(_authorityRepository.Object, _mapper.Object);
        }

        [Fact] public void FindAll_Test()
        {
            _authorityRepository.Setup(mock => mock.FindAll()).Returns(_authorities);
            _authorityService.FindAll().Should().BeEquivalentTo(_authorityDtos);
        }

        [Fact] public void FindById_Test()
        {
            _authorityRepository.Setup(mock => mock.FindById(Constants.ValidAuthorityGuid)).Returns(_authority1);
            _authorityService.FindById(Constants.ValidAuthorityGuid).Should().BeEquivalentTo(_authorityDto1);
        }

        [Fact] public void Save_Test()
        {
            _authorityRepository.Setup(mock => mock.Save(_authority1)).Returns(_authority1);
            _authorityService.Save(_authorityDto1).Should().BeEquivalentTo(_authorityDto1);
        }
    }
}