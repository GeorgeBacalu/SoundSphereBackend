namespace SoundSphere.Database.Dtos.Request
{
    public class PaginationRequest
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}