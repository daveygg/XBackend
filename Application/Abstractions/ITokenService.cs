using Domain.Models;
using System.Security.Claims;

namespace Application.Abstractions;
public interface ITokenService
{
    string CreateToken(User user);
    IEnumerable<Claim> ExtractClaimsFromToken(string token);
    string ExtractClaimValueFromToken(string token, string claimType);
    ClaimsPrincipal GetClaimsPrincipal(string token);
}