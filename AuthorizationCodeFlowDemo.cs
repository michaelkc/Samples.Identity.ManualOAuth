using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;

namespace Seges.Samples.OAuth
{
    internal class AuthorizationCodeFlowDemo
    {
        private ApplicationConfiguration ApplicationConfiguration { get; }

        public AuthorizationCodeFlowDemo(ApplicationConfiguration applicationConfiguration)
        {
            ApplicationConfiguration = applicationConfiguration;
        }

        internal string GenerateAuthorizationCodeUrl()
        {
            var authorizeEndpointUrl = new RequestUrl(this.ApplicationConfiguration.AuthorizeEndpoint);
            var url = authorizeEndpointUrl.CreateAuthorizeUrl(
                clientId: this.ApplicationConfiguration.ClientId,
                scope: this.ApplicationConfiguration.Scope,
                responseType: OidcConstants.ResponseTypes.Code,
                responseMode: OidcConstants.ResponseModes.Fragment,
                redirectUri: this.ApplicationConfiguration.RedirectUri,
                state: CryptoRandom.CreateUniqueId(),
                nonce: CryptoRandom.CreateUniqueId());
            return url;
        }

        private TokenClient CreateTokenClient()
        {
            return new TokenClient(
                address: this.ApplicationConfiguration.TokenEndpoint,
                clientId: this.ApplicationConfiguration.ClientId,
                clientSecret: this.ApplicationConfiguration.ClientSecret
            );
        }
        internal async Task<TokenResponse> ExchangeCodeForRefreshTokenAccessToken(string code)
        {
            var client = CreateTokenClient();
            var codeExchangeResponse = await client.RequestAuthorizationCodeAsync(code, this.ApplicationConfiguration.RedirectUri);
            return codeExchangeResponse;
        }

        internal async Task<TokenResponse> ExchangeRefreshTokenForAccessToken(string refreshToken)
        {
            var client = CreateTokenClient();
            var refreshTokenExchangeResponse = await client.RequestRefreshTokenAsync(refreshToken);
            return refreshTokenExchangeResponse;
        }
    }
}
