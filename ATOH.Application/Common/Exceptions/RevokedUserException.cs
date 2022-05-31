namespace ATOH.Application.Common.Exceptions;

public class RevokedUserException : Exception
{
    public RevokedUserException(string userName, DateTime revokedOn)
    {
        UserName = userName;
        RevokedOn = revokedOn;
    }

    public RevokedUserException(string userName, DateTime revokedOn, string message) : base(message)
    {
        UserName = userName;
        RevokedOn = revokedOn;
    }

    public RevokedUserException(string userName, DateTime revokedOn, string message, Exception innerException) : base(
        message, innerException)
    {
        UserName = userName;
        RevokedOn = revokedOn;
    }

    public string UserName { get; set; }

    public DateTime RevokedOn { get; set; }
}