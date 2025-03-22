using MediatR;

namespace UpdateContact.Application.Contact.Commands.Update;

public sealed record UpdateContactCommand(
        Guid Id,
        string Name,
        int DDDCode,
        string Phone,
        string? Email = null
        ) : IRequest<UpdateContactCommandResponse>;
