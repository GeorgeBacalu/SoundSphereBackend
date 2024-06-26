namespace SoundSphere.Database.Dtos.Response
{
    public record FeedbackStatisticsDto(int? TotalFeedbacks, int? NrIssues, int? NrImprovements, int? NrOptimizations);
}