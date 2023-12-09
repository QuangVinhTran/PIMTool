namespace Application.Exceptions
{
    public class CreateException : Exception
    {
        public CreateException(string message) : base(message) { }
    }
}