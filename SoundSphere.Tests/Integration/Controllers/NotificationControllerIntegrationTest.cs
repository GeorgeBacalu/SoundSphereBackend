using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static Newtonsoft.Json.JsonConvert;
using static SoundSphere.Database.Constants;
using static SoundSphere.Tests.Mocks.NotificationMock;
using static SoundSphere.Tests.Mocks.UserMock;
using static System.Net.HttpStatusCode;

namespace SoundSphere.Tests.Integration.Controllers
{
    public class NotificationControllerIntegrationTest : IDisposable
    {
        private readonly DbFixture _fixture;
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;

        private readonly Notification _notification1 = GetMockedNotification1();
        private readonly Notification _notification2 = GetMockedNotification2();
        private readonly IList<Notification> _notifications = GetMockedNotifications();
        private readonly IList<User> _users = GetMockedUsers();
        private readonly NotificationDto _notificationDto1 = GetMockedNotificationDto1();
        private readonly NotificationDto _notificationDto2 = GetMockedNotificationDto2();
        private readonly IList<NotificationDto> _notificationDtos = GetMockedNotificationDtos();
        private readonly IList<NotificationDto> _paginatedNotificationDtos = GetMockedPaginatedNotificationDtos();
        private readonly NotificationPaginationRequest _paginationRequest = GetMockedNotificationsPaginationRequest();

        public NotificationControllerIntegrationTest()
        {
            _fixture = new DbFixture();
            _factory = new CustomWebAppFactory(_fixture);
            _httpClient = _factory.CreateClient();
        }

        private async Task Execute(Func<Task> action)
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SoundSphereDbContext>();
            await context.Users.AddRangeAsync(_users);
            await context.Notifications.AddRangeAsync(_notifications);
            await context.SaveChangesAsync();
            await action();
            context.Notifications.RemoveRange(context.Notifications);
            context.Users.RemoveRange(context.Users);
            await context.SaveChangesAsync();
        }

        public void Dispose() { _factory.Dispose(); _httpClient.Dispose(); }

        [Fact]
        public async Task GetAll_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PostAsync($"{ApiNotification}/get", new StringContent(SerializeObject(_paginationRequest)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(OK);
            var responseBody = DeserializeObject<IList<NotificationDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_paginatedNotificationDtos);
        });

        [Fact]
        public async Task GetById_ValidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{ApiNotification}/{ValidNotificationGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(OK);
            var responseBody = DeserializeObject<NotificationDto>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(_notificationDto1);
        });

        [Fact]
        public async Task GetById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{ApiNotification}/{InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(NotFound);
            var responseBody = DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = Status404NotFound, Detail = string.Format(NotificationNotFound, InvalidGuid) });
        });

        [Fact]
        public async Task Add_Test() => await Execute(async () =>
        {
            NotificationDto newNotificationDto = GetMockedNotificationDto37();
            var saveResponse = await _httpClient.PostAsync(ApiNotification, new StringContent(SerializeObject(newNotificationDto)));
            saveResponse.Should().NotBeNull();
            saveResponse.StatusCode.Should().Be(Created);
            var saveResponseBody = DeserializeObject<NotificationDto>(await saveResponse.Content.ReadAsStringAsync());
            saveResponseBody.Should().BeEquivalentTo(newNotificationDto, options => options.Excluding(notification => notification.CreatedAt));

            var getAllResponse = await _httpClient.GetAsync(ApiNotification);
            getAllResponse.Should().NotBeNull();
            getAllResponse.StatusCode.Should().Be(OK);
            var getAllResponseBody = DeserializeObject<IList<NotificationDto>>(await getAllResponse.Content.ReadAsStringAsync());
            getAllResponseBody.Should().Contain(newNotificationDto);
        });

        [Fact]
        public async Task UpdateById_ValidId_Test() => await Execute(async () =>
        {
            Notification updatedNotification = new Notification
            {
                Id = ValidNotificationGuid,
                Sender = _notification1.Sender,
                Receiver = _notification1.Receiver,
                Type = _notification2.Type,
                Message = _notification2.Message,
                IsRead = _notification2.IsRead,
                CreatedAt = _notification1.CreatedAt
            };
            NotificationDto updatedNotificationDto = ToDto(updatedNotification);
            var updateResponse = await _httpClient.PutAsync($"{ApiNotification}/{ValidNotificationGuid}", new StringContent(SerializeObject(updatedNotificationDto)));
            updateResponse.Should().NotBeNull();
            updateResponse.StatusCode.Should().Be(OK);
            var updateResponseBody = DeserializeObject<NotificationDto>(await updateResponse.Content.ReadAsStringAsync());
            updateResponseBody.Should().BeEquivalentTo(updatedNotificationDto);

            var getResponse = await _httpClient.GetAsync($"{ApiNotification}/{ValidNotificationGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(OK);
            var getResponseBody = DeserializeObject<NotificationDto>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().BeEquivalentTo(updatedNotificationDto);
        });

        [Fact]
        public async Task UpdateById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PutAsync($"{ApiNotification}/{InvalidGuid}", new StringContent(SerializeObject(_notificationDto2)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(NotFound);
            var responseBody = DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = Status404NotFound, Detail = string.Format(NotificationNotFound, InvalidGuid) });
        });

        [Fact]
        public async Task DeleteById_ValidId_Test() => await Execute(async () =>
        {
            var deleteResponse = await _httpClient.DeleteAsync($"{ApiNotification}/{ValidNotificationGuid}");
            deleteResponse.Should().NotBeNull();
            deleteResponse.StatusCode.Should().Be(NoContent);

            var getResponse = await _httpClient.GetAsync($"{ApiNotification}/{ValidNotificationGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(NotFound);
            var getResponseBody = DeserializeObject<ProblemDetails>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = Status404NotFound, Detail = string.Format(NotificationNotFound, ValidNotificationGuid) });
        });

        [Fact]
        public async Task DeleteById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.DeleteAsync($"{ApiNotification}/{InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(NotFound);
            var responseBody = DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = Status404NotFound, Detail = string.Format(NotificationNotFound, InvalidGuid) });
        });

        private NotificationDto ToDto(Notification notification) => new NotificationDto
        {
            Id = notification.Id,
            SenderId = notification.Sender.Id,
            ReceiverId = notification.Receiver.Id,
            Type = notification.Type,
            Message = notification.Message,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt
        };
    }
}