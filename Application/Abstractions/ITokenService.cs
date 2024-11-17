using Domain.Models;

namespace Application.Abstractions;
public interface ITokenService
{
    string CreateToken(User user);
}