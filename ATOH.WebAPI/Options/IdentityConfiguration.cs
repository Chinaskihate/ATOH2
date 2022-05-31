using IdentityModel;
using IdentityServer4.Models;

namespace ATOH.WebAPI.Options;

public class IdentityConfiguration
{
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new("ATOH Api", "ATOH Api")
        };

    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new("ATOH", "ATOH Api", new[] { JwtClaimTypes.Name })
            {
                Scopes = { "ATOH Api" }
            }
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>();
}