using ATOH.Application.Common.Exceptions;

namespace ATOH.Application.Common.Validators;

public static class UserParamsValidator
{
    public static void CheckUserName(string userName)
    {
        if (string.IsNullOrEmpty(userName))
        {
            throw new ArgumentException("UserName can't be null or empty.", nameof(userName));
        }
    }

    public static void CheckName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name can't be null or empty.", nameof(name));
        }
    }

    public static void CheckBirthday(DateTime birthDay)
    {
        if (birthDay > DateTime.Now)
        {
            throw new ArgumentException("Invalid birthday.", nameof(birthDay));
        }
    }

    public static void CheckIsRevoked(string userName, DateTime? revokedOn)
    {
        if (revokedOn != null)
        {
            throw new RevokedUserException(userName, (DateTime)revokedOn, "User is revoked");
        }
    }
}