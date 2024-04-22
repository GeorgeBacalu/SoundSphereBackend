namespace SoundSphere.Infrastructure.Exceptions
{
    public class ResourceNotFoundException : ApplicationException
    {
        public ResourceNotFoundException(string message) : base(message) { }
    }
}