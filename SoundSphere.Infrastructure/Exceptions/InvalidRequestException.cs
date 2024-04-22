namespace SoundSphere.Infrastructure.Exceptions
{
    public class InvalidRequestException : ApplicationException
    {
        public InvalidRequestException(string message) : base(message) { }
    }
}