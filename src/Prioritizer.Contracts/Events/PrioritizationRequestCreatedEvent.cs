namespace Prioritizer.Contracts.Events;

public record PrioritizationRequestCreatedEvent(
    Guid ConversationId,
    string InitialProposal,
    string ProductName
) : BaseEvent(ConversationId, ProductName);

