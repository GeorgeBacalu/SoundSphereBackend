namespace SoundSphere.Database.Dtos.Request
{
    public record PaginationRequest(int Page = 0, int Size = 10);
}