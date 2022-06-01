namespace ATOH.WebAPI.ViewModels;

/// <summary>
/// ViewModel for Login operation.
/// </summary>
public class LoginViewModel
{

    /// <summary>
    /// UserName for Login.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Password for Login.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}