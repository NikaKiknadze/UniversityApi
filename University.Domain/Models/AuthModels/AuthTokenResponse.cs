namespace University.Domain.Models.AuthModels;

public class AuthTokenResponse : TokenResponseBase
{
    public string? AccessToken { get; set; }
    public string? TokenType { get; set; }
    public long? ExpiresIn { get; set; }
    public string? RefreshToken { get; set; }
    public long? RefreshTokenExpiresIn { get; set; }
}