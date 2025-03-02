namespace University.Domain.Models.AuthModels;

public class TokenResponseBase
{
    public bool Error { get; set; }
    public string? ErrorType { get; set; }
    public string? ErrorMessage { get; set; }
}