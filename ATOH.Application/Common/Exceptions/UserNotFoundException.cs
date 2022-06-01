namespace ATOH.Application.Common.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string userName)
    {
        UserName = userName;
    }

    public UserNotFoundException(string userName, string message) : base(message)
    {
        UserName = userName;
    }

    public UserNotFoundException(string userName, string message, Exception innerException) : base(
        message, innerException)
    {
        UserName = userName;
    }

    public string UserName { get; set; }
}