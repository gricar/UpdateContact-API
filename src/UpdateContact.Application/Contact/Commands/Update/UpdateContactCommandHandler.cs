using MediatR;
using UpdateContact.Application.Common.Exceptions;
using UpdateContact.Application.Common.Messaging;
using UpdateContact.Application.Common.Messaging.Events;
using UpdateContact.Application.Persistence;

namespace UpdateContact.Application.Contact.Commands.Update;

public class UpdateContactCommandHandler(
    IContactRepository contactRepository, IEventBus eventBus)
    : IRequestHandler<UpdateContactCommand, UpdateContactCommandResponse>
{
    public async Task<UpdateContactCommandResponse> Handle(UpdateContactCommand command, CancellationToken cancellationToken)
    {
        var contact = await contactRepository.GetAsync(command.Id);

        if (contact is null)
        {
            throw new ContactNotFoundException(command.Id);
        }

        await EnsureContactIsUpdatableAsync(command, contact);

        var contactEvent = new ContactUpdatedEvent(
            contact.Id,
            contact.Name,
            contact.Region.DddCode,
            contact.Phone,
            contact.Email
        );

        await eventBus.PublishAsync(contactEvent, "contact-updated");

        return new UpdateContactCommandResponse("Contact updated request accepted and is being processed.");
    }

    private async Task EnsureContactIsUpdatableAsync(UpdateContactCommand command, Domain.Entities.Contact existingContact)
    {
        if (HasPhoneChanged(command, existingContact))
        {
            await CheckForUniqueContactAsync(command.DDDCode, command.Phone);

            existingContact.UpdateRegion(command.DDDCode);
            existingContact.UpdatePhone(command.Phone);
        }

        if (!string.IsNullOrWhiteSpace(command.Email) && HasEmailChanged(command.Email!, existingContact.Email!))
        {
            await CheckForUniqueEmailAsync(command.Email!);
            existingContact.UpdateEmail(command.Email);
        }

        if (HasNameChanged(command.Name, existingContact.Name))
        {
            existingContact.UpdateName(command.Name);
        }
    }

    private bool HasPhoneChanged(UpdateContactCommand command, Domain.Entities.Contact existingContact) =>
        command.DDDCode != existingContact.Region.DddCode ||
        command.Phone != existingContact.Phone;

    private bool HasEmailChanged(string newEmail, string existingEmail) =>
        !string.Equals(newEmail, existingEmail, StringComparison.OrdinalIgnoreCase);

    private bool HasNameChanged(string newName, string existingName) =>
        !string.Equals(newName, existingName, StringComparison.OrdinalIgnoreCase);

    private async Task CheckForUniqueEmailAsync(string email)
    {
        if (await contactRepository.IsEmailUniqueAsync(email))
        {
            throw new DuplicateEmailException(email!);
        }
    }

    private async Task CheckForUniqueContactAsync(int dddCode, string phone)
    {
        if (await contactRepository.IsDddAndPhoneUniqueAsync(dddCode, phone))
        {
            throw new DuplicateContactException(dddCode, phone);
        }
    }
}
