namespace SoundSphere.Infrastructure.Exceptions
{
    public class ForbiddenAccessException : ApplicationException
    {
        public ForbiddenAccessException(string message) : base(message) { }
    }
}