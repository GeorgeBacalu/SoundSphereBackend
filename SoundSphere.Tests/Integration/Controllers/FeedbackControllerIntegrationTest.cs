using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SoundSphere.Database.Constants;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Tests.Mocks;
using System.Net;
using System.Text;

namespace SoundSphere.Tests.Integration.Controllers
{
    public class FeedbackControllerIntegrationTest : IDisposable
    {
        private readonly DbFixture _fixture;
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;

        private readonly Feedback _feedback1 = FeedbackMock.GetMockedFeedback1();
        private readonly Feedback _feedback2 = FeedbackMock.GetMockedFeedback2();
        private readonly IList<Feedback> _feedbacks = FeedbackMock.GetMockedFeedbacks();
        private readonly IList<User> _users = UserMock.GetMockedUsers();
        private readonly FeedbackDto _feedbackDto1 = FeedbackMock.GetMockedFeedbackDto1();
        private readonly FeedbackDto _feedbackDto2 = FeedbackMock.GetMockedFeedbackDto2();
        private readonly IList<FeedbackDto> _feedbackDtos = FeedbackMock.GetMockedFeedbackDtos();

        public FeedbackControllerIntegrationTest()
        {
            _fixture = new DbFixture();
            _factory = new CustomWebAppFactory(_fixture);
            _httpClient = _factory.CreateClient();
        }

        private async Task Execute(Func<Task> action)
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SoundSphereContext>();
            await context.Users.AddRangeAsync(_users);
            await context.Feedbacks.AddRangeAsync(_feedbacks);
            await context.SaveChangesAsync();
            await action();
            context.Feedbacks.RemoveRange(context.Feedbacks);
            context.Users.RemoveRange(context.Users);
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _factory.Dispose();
            _httpClient.Dispose();
        }

        [Fact] public async Task FindAll_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync(Constants.ApiFeedback);
            response?.Should().NotBeNull();
            response?.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<IList<FeedbackDto>>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(_feedbackDtos);
        });

        [Fact] public async Task FindById_ValidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiFeedback}/{Constants.ValidFeedbackGuid}");
            response?.Should().NotBeNull();
            response?.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<FeedbackDto>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(_feedbackDto1);
        });

        [Fact] public async Task FindById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.GetAsync($"{Constants.ApiFeedback}/{Constants.InvalidGuid}");
            response?.Should().NotBeNull();
            response?.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(new ProblemDetails
            {
                Title = "Resource not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Feedback with id {Constants.InvalidGuid} not found!",
            });
        });

        [Fact] public async Task Save_Test() => await Execute(async () =>
        {
            FeedbackDto newFeedbackDto = FeedbackMock.GetMockedFeedbackDto3();
            var requestBody = new StringContent(JsonConvert.SerializeObject(newFeedbackDto), Encoding.UTF8, "application/json");
            var saveResponse = await _httpClient.PostAsync(Constants.ApiFeedback, requestBody);
            saveResponse?.Should().NotBeNull();
            saveResponse?.StatusCode.Should().Be(HttpStatusCode.Created);
            var saveResult = JsonConvert.DeserializeObject<FeedbackDto>(await saveResponse.Content.ReadAsStringAsync());
            saveResult?.Id.Should().Be(newFeedbackDto.Id);
            saveResult?.Type.Should().Be(newFeedbackDto.Type);
            saveResult?.Message.Should().Be(newFeedbackDto.Message);
            saveResult?.UserId.Should().Be(newFeedbackDto.UserId);

            var getAllResponse = await _httpClient.GetAsync(Constants.ApiFeedback);
            getAllResponse?.Should().NotBeNull();
            getAllResponse?.StatusCode.Should().Be(HttpStatusCode.OK);
            var getAllResult = JsonConvert.DeserializeObject<IList<FeedbackDto>>(await getAllResponse.Content.ReadAsStringAsync());
            getAllResult.Should().Contain(newFeedbackDto);
        });

        [Fact] public async Task UpdateById_ValidId_Test() => await Execute(async () =>
        {
            Feedback updatedFeedback = new Feedback
            {
                Id = Constants.ValidFeedbackGuid,
                User = _feedback1.User,
                Type = _feedback2.Type,
                Message = _feedback2.Message,
                SentAt = _feedback1.SentAt
            };
            FeedbackDto updatedFeedbackDto = ConvertToDto(updatedFeedback);
            var requestBody = new StringContent(JsonConvert.SerializeObject(_feedbackDto2), Encoding.UTF8, "application/json");
            var updateResponse = await _httpClient.PutAsync($"{Constants.ApiFeedback}/{Constants.ValidFeedbackGuid}", requestBody);
            updateResponse?.Should().NotBeNull();
            updateResponse?.StatusCode.Should().Be(HttpStatusCode.OK);
            var updateResult = JsonConvert.DeserializeObject<FeedbackDto>(await updateResponse.Content.ReadAsStringAsync());
            updateResult.Should().BeEquivalentTo(updatedFeedbackDto);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiFeedback}/{Constants.ValidFeedbackGuid}");
            getResponse?.Should().NotBeNull();
            getResponse?.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResult = JsonConvert.DeserializeObject<FeedbackDto>(await getResponse.Content.ReadAsStringAsync());
            getResult.Should().BeEquivalentTo(updatedFeedbackDto);
        });

        [Fact] public async Task UpdateById_InvalidId_Test() => await Execute(async () =>
        {
            var requestBody = new StringContent(JsonConvert.SerializeObject(_feedbackDto2), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{Constants.ApiFeedback}/{Constants.InvalidGuid}", requestBody);
            response?.Should().NotBeNull();
            response?.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(new ProblemDetails
            {
                Title = "Resource not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Feedback with id {Constants.InvalidGuid} not found!",
            });
        });

        [Fact] public async Task DeleteById_ValidId_Test() => await Execute(async () =>
        {
            var deleteResponse = await _httpClient.DeleteAsync($"{Constants.ApiFeedback}/{Constants.ValidFeedbackGuid}");
            deleteResponse?.Should().NotBeNull();
            deleteResponse?.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _httpClient.GetAsync($"{Constants.ApiFeedback}/{Constants.ValidFeedbackGuid}");
            getResponse?.Should().NotBeNull();
            getResponse?.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var getResult = JsonConvert.DeserializeObject<ProblemDetails>(await getResponse.Content.ReadAsStringAsync());
            getResult.Should().BeEquivalentTo(new ProblemDetails
            {
                Title = "Resource not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Feedback with id {Constants.ValidFeedbackGuid} not found!",
            });
        });

        [Fact] public async Task DeleteById_InvalidId_Test() => await Execute(async () =>
        {
            var response = await _httpClient.DeleteAsync($"{Constants.ApiFeedback}/{Constants.InvalidGuid}");
            response?.Should().NotBeNull();
            response?.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(new ProblemDetails
            {
                Title = "Resource not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Feedback with id {Constants.InvalidGuid} not found!",
            });
        });

        private FeedbackDto ConvertToDto(Feedback feedback) => new FeedbackDto
        {
            Id = feedback.Id,
            UserId = feedback.User.Id,
            Type = feedback.Type,
            Message = feedback.Message,
            SentAt = feedback.SentAt
        };
    }
}