using MediatR;

namespace HealthMed.Application.Features.Login;

public class LoginRequest : IRequest<string>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
