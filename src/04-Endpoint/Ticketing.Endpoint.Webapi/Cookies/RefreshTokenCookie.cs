using Ticketing.Application.DTOs.Auth;

namespace Ticketing.Endpoint.Webapi.Cookies;

public static class RefreshTokenCookie
{
    private const string Name = "ticketing.refresh";
    private const string Path = "/api/Auth";

    public static bool TryRead(HttpRequest request, out string refreshToken) =>
        request.Cookies.TryGetValue(Name, out refreshToken!) && !string.IsNullOrWhiteSpace(refreshToken);

    public static void Set(HttpResponse response, AuthTokenDto tokens) =>
        response.Cookies.Append(Name, tokens.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = Path,
            Expires = new DateTimeOffset(DateTime.SpecifyKind(tokens.RefreshTokenExpiresAtUtc, DateTimeKind.Utc))
        });

    public static void Clear(HttpResponse response) =>
        response.Cookies.Delete(Name, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = Path
        });
}
