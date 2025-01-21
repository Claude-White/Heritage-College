using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace OHMS;

public class HIAAAStateProvider : AuthenticationStateProvider {
    private readonly HttpClient _http;
    private readonly CookieStorageAccessor _cookieStorageAccessor;

    public HIAAAStateProvider(HttpClient http, CookieStorageAccessor cookieStorageAccessor) {
        _http = http;
        _cookieStorageAccessor = cookieStorageAccessor;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
        var token = await _cookieStorageAccessor.GetValueAsync<string>("token");
        var rolesJson = await _cookieStorageAccessor.GetValueAsync<string>("roles");
        
        var identity = new ClaimsIdentity();
        _http.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrEmpty(token)) {
            identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            
            var roles = string.IsNullOrEmpty(rolesJson) 
                ? new List<string>() 
                : JsonSerializer.Deserialize<List<string>>(rolesJson);
            foreach (var role in roles) {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
        }

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }
    
    public async Task LogoutAsync() {
        await _cookieStorageAccessor.DeleteValueAsync("token");
        await _cookieStorageAccessor.DeleteValueAsync("roles");
        
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt) {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    private static byte[] ParseBase64WithoutPadding(string base64) {
        switch (base64.Length % 4) {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }

        return Convert.FromBase64String(base64);
    }
}