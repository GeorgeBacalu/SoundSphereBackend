using AutoMapper;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Mappings
{
    public static class FeedbackMappingExtensions
    {
        public static IList<FeedbackDto> ToDtos(this IList<Feedback> feedbacks, IMapper mapper) => feedbacks.Select(feedback => feedback.ToDto(mapper)).ToList();

        public static IList<Feedback> ToEntities(this IList<FeedbackDto> feedbackDtos, IUserRepository userRepository, IMapper mapper) => feedbackDtos.Select(feedbackDto => feedbackDto.ToEntity(userRepository, mapper)).ToList();

        public static FeedbackDto ToDto(this Feedback feedback, IMapper mapper) => mapper.Map<FeedbackDto>(feedback);

        public static Feedback ToEntity(this FeedbackDto feedbackDto, IUserRepository userRepository, IMapper mapper)
        {
            Feedback feedback = mapper.Map<Feedback>(feedbackDto);
            feedback.User = userRepository.FindById(feedbackDto.UserId);
            return feedback;
        }
    }
}