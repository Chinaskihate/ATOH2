using IdentityModel;
using IdentityServer4.Models;

namespace ATOH.WebAPI.Options;

/// <summary>
/// Configure IdentityServer.
/// </summary>
public static class IdentityConfiguration
{
    /// <summary>
    /// ApiScopes.
    /// </summary>
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new("ATOH Api", "ATOH Api")
        };

    /// <summary>
    /// IdentityResources.
    /// </summary>
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    /// <summary>
    /// ApiResources.
    /// </summary>
    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new("ATOH", "ATOH Api", new[] { JwtClaimTypes.Name })
            {
                Scopes = { "ATOH Api" }
            }
        };

    /// <summary>
    /// App clients.
    /// </summary>
    public static IEnumerable<Client> Clients =>
        new List<Client>();
}