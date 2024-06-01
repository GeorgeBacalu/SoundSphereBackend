using AutoMapper;
using FluentAssertions;
using Moq;
using SoundSphere.Core.Services;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.AuthorityMock;

namespace SoundSphere.Tests.Unit.Services
{
    public class AuthorityServiceTest
    {
        private readonly Mock<IAuthorityRepository> _authorityRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly IAuthorityService _authorityService;

        private readonly Authority _authority1 = GetMockedAuthority1();
        private readonly Authority _authority2 = GetMockedAuthority2();
        private readonly Authority _authority3 = GetMockedAuthority3();
        private readonly Authority _authority4 = GetMockedAuthority4();
        private readonly IList<Authority> _authorities = GetMockedAuthorities();
        private readonly AuthorityDto _authorityDto1 = GetMockedAuthorityDto1();
        private readonly AuthorityDto _authorityDto2 = GetMockedAuthorityDto2();
        private readonly AuthorityDto _authorityDto3 = GetMockedAuthorityDto3();
        private readonly AuthorityDto _authorityDto4 = GetMockedAuthorityDto4();
        private readonly IList<AuthorityDto> _authorityDtos = GetMockedAuthorityDtos();

        public AuthorityServiceTest()
        {
            _mapperMock.Setup(mock => mock.Map<AuthorityDto>(_authority1)).Returns(_authorityDto1);
            _mapperMock.Setup(mock => mock.Map<AuthorityDto>(_authority2)).Returns(_authorityDto2);
            _mapperMock.Setup(mock => mock.Map<AuthorityDto>(_authority3)).Returns(_authorityDto3);
            _mapperMock.Setup(mock => mock.Map<AuthorityDto>(_authority4)).Returns(_authorityDto4);
            _mapperMock.Setup(mock => mock.Map<Authority>(_authorityDto1)).Returns(_authority1);
            _mapperMock.Setup(mock => mock.Map<Authority>(_authorityDto2)).Returns(_authority2);
            _mapperMock.Setup(mock => mock.Map<Authority>(_authorityDto3)).Returns(_authority3);
            _mapperMock.Setup(mock => mock.Map<Authority>(_authorityDto4)).Returns(_authority4);
            _authorityService = new AuthorityService(_authorityRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact] public void GetAll_Test()
        {
            _authorityRepositoryMock.Setup(mock => mock.GetAll()).Returns(_authorities);
            _authorityService.GetAll().Should().BeEquivalentTo(_authorityDtos);
        }

        [Fact] public void GetById_Test()
        {
            _authorityRepositoryMock.Setup(mock => mock.GetById(ValidAuthorityGuid)).Returns(_authority1);
            _authorityService.GetById(ValidAuthorityGuid).Should().Be(_authorityDto1);
        }

        [Fact] public void Add_Test()
        {
            _authorityRepositoryMock.Setup(mock => mock.Add(_authority1)).Returns(_authority1);
            _authorityService.Add(_authorityDto1).Should().Be(_authorityDto1);
        }
    }
}