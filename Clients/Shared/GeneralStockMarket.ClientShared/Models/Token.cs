namespace GeneralStockMarket.ClientShared.Models
{
    public record Token(string AccessToken, int ExpiresIn, string TokenType, string Scope, string RefreshToken);
}
