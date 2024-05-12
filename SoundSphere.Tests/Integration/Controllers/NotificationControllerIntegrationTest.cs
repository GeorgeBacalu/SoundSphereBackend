using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SoundSphere.Database;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Tests.Mocks;
using System.Net;

namespace SoundSphere.Tests.Integration.Controllers
{
    public class NotificationControllerIntegrationTest : IDisposable
    {
        private readonly DbFixture _fixture;
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;

        private readonly Notification _notification1 = NotificationMock.GetMockedNotification1();
        private readonly Notification _notification2 = NotificationMock.GetMockedNotification2();
        private readonly IList<Notification> _notifications = NotificationMock.GetMockedNotifications();
        private readonly IList<User> _users = UserMock.GetMockedUsers();
        private readonly NotificationDto _notificationDto1 = NotificationMock.GetMockedNotificationDto1();
        private readonly NotificationDto _notificationDto2 = NotificationMock.GetMockedNotificationDto2();
        private readonly IList<NotificationDto> _notificationDtos = NotificationMock.GetMockedNotificationDtos();
        private readonly IList<NotificationDto> _paginatedNotificationDtos = NotificationMock.GetMockedPaginatedNotificationDtos();
        private readonly NotificationPaginationRequest _paginationRequest = NotificationMock.GetMockedPaginationRequest();

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

        [Fact] public async Task FindAll_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync(Constants.ApiNotification);
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<IList<NotificationDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_notificationDtos);
        });

        [Fact] public async Task FindAllPagination_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PostAsync($"{Constants.ApiNotification}/pagination", new StringContent(JsonConvert.SerializeObject(_paginationRequest)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<IList<NotificationDto>>(await response.Content.ReadAsStringAsync());
            responseBody.Should().BeEquivalentTo(_paginatedNotificationDtos);
        });

        [Fact] public async Task FindById_ValidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiNotification}/{Constants.ValidNotificationGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = JsonConvert.DeserializeObject<NotificationDto>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(_notificationDto1);
        });

        [Fact] public async Task FindById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiNotification}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseBody = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.NotificationNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task Save_Test() => await Execute(async () =>
        {
            NotificationDto newNotificationDto = NotificationMock.GetMockedNotificationDto3();
            var saveResponse = await _httpClient.PostAsync(Constants.ApiNotification, new StringContent(JsonConvert.SerializeObject(newNotificationDto)));
            saveResponse.Should().NotBeNull();
            saveResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var saveResponseBody = JsonConvert.DeserializeObject<NotificationDto>(await saveResponse.Content.ReadAsStringAsync());
            saveResponseBody.Should().BeEquivalentTo(newNotificationDto, options => options.Excluding(notification => notification.SentAt));

            var getAllResponse = await _httpClient.GetAsync(Constants.ApiNotification);
            getAllResponse.Should().NotBeNull();
            getAllResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getAllResponseBody = JsonConvert.DeserializeObject<IList<NotificationDto>>(await getAllResponse.Content.ReadAsStringAsync());
            getAllResponseBody.Should().Contain(newNotificationDto);
        });

        [Fact] public async Task UpdateById_ValidId_Test() => await Execute(async () =>
        {
            Notification updatedNotification = new Notification
            {
                Id = Constants.ValidNotificationGuid,
                User = _notification1.User,
                Type = _notification2.Type,
                Message = _notification2.Message,
                SentAt = _notification1.SentAt,
                IsRead = _notification2.IsRead
            };
            NotificationDto updatedNotificationDto = ToDto(updatedNotification);
            var updateResponse = await _httpClient.PutAsync($"{Constants.ApiNotification}/{Constants.ValidNotificationGuid}", new StringContent(JsonConvert.SerializeObject(updatedNotificationDto)));
            updateResponse.Should().NotBeNull();
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updateResponseBody = JsonConvert.DeserializeObject<NotificationDto>(await updateResponse.Content.ReadAsStringAsync());
            updateResponseBody.Should().BeEquivalentTo(updatedNotificationDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiNotification}/{Constants.ValidNotificationGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResponseBody = JsonConvert.DeserializeObject<NotificationDto>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().BeEquivalentTo(updatedNotificationDto);
        });

        [Fact] public async Task UpdateById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.PutAsync($"{Constants.ApiNotification}/{Constants.InvalidGuid}", new StringContent(JsonConvert.SerializeObject(_notificationDto2)));
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseBody = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.NotificationNotFound, Constants.InvalidGuid) });
        });

        [Fact] public async Task DeleteById_ValidId_Test() => await Execute(async () =>
        {
            var deleteResponse = await _httpClient.DeleteAsync($"{Constants.ApiNotification}/{Constants.ValidNotificationGuid}");
            deleteResponse.Should().NotBeNull();
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiNotification}/{Constants.ValidNotificationGuid}");
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var getResponseBody = JsonConvert.DeserializeObject<ProblemDetails>(await getResponse.Content.ReadAsStringAsync());
            getResponseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.NotificationNotFound, Constants.ValidNotificationGuid) });
        });

        [Fact] public async Task DeleteById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.DeleteAsync($"{Constants.ApiNotification}/{Constants.InvalidGuid}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseBody = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            responseBody.Should().Be(new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = string.Format(Constants.NotificationNotFound, Constants.InvalidGuid) });
        });

        private NotificationDto ToDto(Notification notification) => new NotificationDto
        {
            Id = notification.Id,
            UserId = notification.User.Id,
            Type = notification.Type,
            Message = notification.Message,
            SentAt = notification.SentAt,
            IsRead = notification.IsRead
        };
    }
}