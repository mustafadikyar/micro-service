using IdentityModel.Client;
using Micro.Shared.DTOs;
using Micro.WebUI.Models;
using Micro.WebUI.Services.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Micro.WebUI.Services
{
    public class IdentityManager : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;

        public IdentityManager(HttpClient client, IHttpContextAccessor httpContextAccessor, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _httpClient = client;
            _httpContextAccessor = httpContextAccessor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
        }

        public async Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            DiscoveryDocumentResponse disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new() { RequireHttps = false }
            });

            if (disco.IsError)
                throw disco.Exception;

            string refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            RefreshTokenRequest refreshTokenRequest = new()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                RefreshToken = refreshToken,
                Address = disco.TokenEndpoint
            };

            TokenResponse token = await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if (token.IsError)
                return null;

            List<AuthenticationToken> authenticationTokens = new()
            {
                new() { Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken },
                new() { Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken },
                new() { Name = OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o", CultureInfo.InvariantCulture) }
            };

            AuthenticateResult authenticationResult = await _httpContextAccessor.HttpContext.AuthenticateAsync();

            AuthenticationProperties properties = authenticationResult.Properties;
            properties.StoreTokens(authenticationTokens);

            await _httpContextAccessor.HttpContext.SignInAsync( CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal, properties );

            return token;
        }

        public async Task RevokeRefreshToken()
        {
            DiscoveryDocumentResponse disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new() { RequireHttps = false }
            });

            if (disco.IsError)
                throw disco.Exception;

            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            TokenRevocationRequest tokenRevocationRequest = new()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                Address = disco.RevocationEndpoint,
                Token = refreshToken,
                TokenTypeHint = "refresh_token"
            };

            await _httpClient.RevokeTokenAsync(tokenRevocationRequest);
        }

        public async Task<Response<bool>> SignIn(SigninInput model)
        {
            //IdentityServer endpoints
            DiscoveryDocumentResponse disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new() { RequireHttps = false }
            });

            if (disco.IsError)
                throw disco.Exception;

            TokenResponse token = await _httpClient.RequestPasswordTokenAsync(request: new PasswordTokenRequest
            {
                ClientId = _clientSettings.WebClientForUser.ClientId, //sabit
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret, //sabit
                UserName = model.Email, //hata var ise ya email ya password hatalıdır. 
                Password = model.Password, //diğer alanlar sabit olduğu için
                Address = disco.TokenEndpoint // sabit
            });

            if (token.IsError)
            {
                string responseContent = await token.HttpResponse.Content.ReadAsStringAsync();
                ErrorDTO errorDto = JsonSerializer.Deserialize<ErrorDTO>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return Response<bool>.Error(errorDto.Errors, 400);
            }

            UserInfoResponse userInfo = await _httpClient.GetUserInfoAsync(new UserInfoRequest
            {
                Token = token.AccessToken,
                Address = disco.UserInfoEndpoint
            });

            if (userInfo.IsError)
                throw userInfo.Exception;

            ClaimsIdentity claimsIdentity = new(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");
            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

            AuthenticationProperties authenticationProperties = new();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                //token
                new AuthenticationToken{
                    Name=OpenIdConnectParameterNames.AccessToken,
                    Value=token.AccessToken
                },
                //refresh token
                new AuthenticationToken{
                    Name=OpenIdConnectParameterNames.RefreshToken,
                    Value=token.RefreshToken
                },
                //expires date
                new AuthenticationToken{
                    Name=OpenIdConnectParameterNames.ExpiresIn,
                    Value= DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)
                }
            });

            //kalıcı olacak
            authenticationProperties.IsPersistent = model.IsRemember;

            //login - cookie oluştur
            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                authenticationProperties
                );

            return Response<bool>.Success(200);
        }
    }
}
