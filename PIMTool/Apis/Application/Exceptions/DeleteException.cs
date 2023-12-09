namespace Application.Exceptions
{
    public class DeleteException : Exception
    {
        public DeleteException(string message) : base(message) { }
    }
}