using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using Okta.AspNetCore;

namespace Mvp.Feature.User.Helpers
{
    public class RefreshTokenHelper
    {
        private static readonly HttpClient Client = new ();

        private readonly OktaMvcOptions _options;

        public RefreshTokenHelper(OktaMvcOptions options)
        {
            _options = options;
        }

        public async Task OnValidatePrincipal(CookieValidatePrincipalContext context)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            string expiresAt = context.Properties.GetTokenValue("expires_at") ?? DateTimeOffset.MinValue.ToString();
            DateTimeOffset tokenExpiration = DateTimeOffset.Parse(expiresAt);
            TimeSpan timeRemaining = tokenExpiration.Subtract(now);
            TimeSpan refreshThreshold = TimeSpan.FromMinutes(5);
            if (timeRemaining < refreshThreshold)
            {
                string refreshToken = context.Properties.GetTokenValue("refresh_token");
                FormUrlEncodedContent reqContent = new (new[]
                {
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("scope", "offline_access openid"),
                    new KeyValuePair<string, string>("refresh_token", refreshToken),
                    new KeyValuePair<string, string>("client_id", _options.ClientId),
                    new KeyValuePair<string, string>("client_secret", _options.ClientSecret)
                });
                HttpResponseMessage response = await Client.PostAsync($"{_options.OktaDomain}/oauth2/{_options.AuthorizationServerId}/v1/token", reqContent);
                if (response.IsSuccessStatusCode)
                {
                    RefreshTokenResponse tokenResponse =
                        JsonConvert.DeserializeObject<RefreshTokenResponse>(
                            await response.Content.ReadAsStringAsync()) ?? new RefreshTokenResponse();
                    DateTimeOffset updatedExpiresAt = DateTimeOffset.UtcNow.AddSeconds(tokenResponse.ExpiresIn);
                    context.Properties.UpdateTokenValue("expires_at", updatedExpiresAt.ToString());
                    context.Properties.UpdateTokenValue("access_token", tokenResponse.AccessToken);
                    context.Properties.UpdateTokenValue("refresh_token", tokenResponse.RefreshToken);
                    context.ShouldRenew = true;
                }
                else
                {
                    context.RejectPrincipal();
                    await context.HttpContext.SignOutAsync();
                }
            }
        }

        private class RefreshTokenResponse
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            [JsonProperty("token_type")]
            public string TokenType { get; set; }

            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; }

            [JsonProperty("scope")]
            public string Scope { get; set; }

            [JsonProperty("refresh_token")]
            public string RefreshToken { get; set; }

            [JsonProperty("id_token")]
            public string IdToken { get; set; }
        }
    }
}
