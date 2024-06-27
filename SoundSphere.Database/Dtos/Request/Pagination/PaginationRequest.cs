namespace SoundSphere.Database.Dtos.Request.Pagination
{
    public record PaginationRequest(int Page = 0, int Size = 10);
}