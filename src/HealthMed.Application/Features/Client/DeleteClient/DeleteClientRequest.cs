using FluentValidation;
using MediatR;

namespace HealthMed.Application.Features.Client.DeleteClient;

public class DeleteClientRequest : IRequest
{
    public DeleteClientRequest(Guid clientID)
    {
        ClientID = clientID;
    }

    public Guid ClientID { get; }
}

public class DeleteClientRequestValidator : AbstractValidator<DeleteClientRequest>
{
    public DeleteClientRequestValidator()
    {
        RuleFor(x => x.ClientID).NotEmpty().NotNull();
    }
}
