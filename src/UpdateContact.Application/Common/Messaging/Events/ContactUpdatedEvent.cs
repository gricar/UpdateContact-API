namespace UpdateContact.Application.Common.Messaging.Events;

public record ContactUpdatedEvent(
    Guid ContactId,
    string Name,
    int DDDCode,
    string Phone,
    string? Email
    ) : IntegrationEvent;