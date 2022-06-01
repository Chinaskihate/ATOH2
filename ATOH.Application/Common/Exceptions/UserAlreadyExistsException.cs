namespace ATOH.Application.Common.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string userName)
    {
        UserName = userName;
    }

    public UserAlreadyExistsException(string userName, string message) : base(message)
    {
        UserName = userName;
    }

    public UserAlreadyExistsException(string userName, string message, Exception innerException) : base(
        message, innerException)
    {
        UserName = userName;
    }

    public string UserName { get; set; }
}