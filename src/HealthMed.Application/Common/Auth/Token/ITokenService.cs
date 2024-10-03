using HealthMed.Domain.Entities;

namespace HealthMed.Application.Common.Auth.Token;

public interface ITokenService
{
    string GetUserToken(User usuario);
}
