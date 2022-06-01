namespace ATOH.Application.Users.ChangePassword;

public class ChangePasswordByAdminDto
{

    public string UserName { get; set; } = string.Empty;

    public string NewPassword { get; set; } = string.Empty;
}