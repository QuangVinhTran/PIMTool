namespace Application.Exceptions;
public class ProjectNumberAlreadyExistsException : Exception
{
    public ProjectNumberAlreadyExistsException(string message) : base(message)
    {
        {
        }
    }
}