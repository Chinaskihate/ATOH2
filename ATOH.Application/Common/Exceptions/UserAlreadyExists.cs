namespace ATOH.Application.Common.Exceptions;

public class UserAlreadyExists : Exception
{
    public UserAlreadyExists(string userName)
    {
        UserName = userName;
    }

    public UserAlreadyExists(string userName, string message) : base(message)
    {
        UserName = userName;
    }

    public UserAlreadyExists(string userName, string message, Exception innerException) : base(
        message, innerException)
    {
        UserName = userName;
    }

    public string UserName { get; set; }
}