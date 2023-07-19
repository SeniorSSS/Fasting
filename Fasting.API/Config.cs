using IdentityServer4.Models;

namespace Fasting.Host
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource> { new IdentityResources.OpenId(), new IdentityResources.Profile() };

        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope> { new ApiScope("api1", "My API") };

        public static IEnumerable<Client> GetClients() => new List<Client>
    {
        new Client
        {
            ClientId = "client",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets = { new Secret("secret".Sha256()) },
            AllowedScopes = { "api1" }
        }
    };
    }
}
