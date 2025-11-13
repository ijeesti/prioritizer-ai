namespace Prioritizer.Contracts.Events;

public record PrioritizationDecisionMadeEvent(
    Guid ConversationId,
    string ProductName,
    string FinalDecision,
    string Rationale,
    string FullHistory
) : BaseEvent(ConversationId, ProductName);