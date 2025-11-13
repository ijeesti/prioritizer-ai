namespace Prioritizer.Contracts.Events;

public record AgentInputProvidedInput(
    Guid ConversationId,
    string AgentName,
    string Input,
    string ProductName
) : BaseEvent(ConversationId, ProductName);