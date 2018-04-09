using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;

namespace Seges.Samples.OAuth
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var applicationConfiguration = ReadConfiguration();
            var demo = new AuthorizationCodeFlowDemo(applicationConfiguration);
            Console.WriteLine("Paste this url in browser, authenticate, and paste the resulting browser url back here:");
            Console.WriteLine("####################################################################################");
            var authorizeUrl = demo.GenerateAuthorizationCodeUrl();
            Console.WriteLine(authorizeUrl);
            Console.WriteLine("####################################################################################");
            Console.WriteLine("The browser will redirect to the return url, containing the authorization code in the fragment.");
            Console.WriteLine("Enter the return url with fragment:");
            var callbackUrlWithCode = Console.ReadLine();
            var codeResponse = new AuthorizeResponse(callbackUrlWithCode);
            Console.WriteLine($"Exchanging authorization code {codeResponse.Code} for refresh/access tokens");
            var tokenResponse = await demo.ExchangeCodeForRefreshTokenAccessToken(codeResponse.Code);
            DumpTokenResponse(tokenResponse);
            Console.WriteLine($"Exchanging refresh token {tokenResponse.RefreshToken} for fresh refresh/access tokens");
            var tokenResponse2 = await demo.ExchangeRefreshTokenForAccessToken(tokenResponse.RefreshToken);
            DumpTokenResponse(tokenResponse2);

            Console.WriteLine($"Making fake API call with access token {tokenResponse2.AccessToken} as Bearer token in Authorization header");
            var httpClient = new HttpClient();
            httpClient.SetBearerToken(tokenResponse2.AccessToken);
            var apiResult = await httpClient.GetAsync(applicationConfiguration.ApiEndpoint);
        }

        private static ApplicationConfiguration ReadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{nameof(ApplicationConfiguration)}.json");

            var config = builder.Build();
            var section = config.GetSection(nameof(ApplicationConfiguration));

            var applicationConfiguration = section.Get<ApplicationConfiguration>();
            return applicationConfiguration;
        }

        private static void DumpTokenResponse(TokenResponse tokenResponse)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenResponse.AccessToken);
            Console.WriteLine("####################################################################################");
            Console.WriteLine("Token claims");
            foreach (var claim in token.Claims)
            {
                Console.WriteLine($"{claim.Type} : {claim.Value}");
            }
            Console.WriteLine($"Expires: {tokenResponse.ExpiresIn}");
            Console.WriteLine($"Access token: {tokenResponse.AccessToken}");
            Console.WriteLine($"Refresh token: {tokenResponse.RefreshToken}");
            Console.WriteLine("####################################################################################");
        }
    }
}
