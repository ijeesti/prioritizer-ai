namespace Prioritizer.Contracts.Events;

public record RequestAnalysisCompletedEvent(
    Guid ConversationId,
    string ProductName,
    string InitialProposal,
    string PolicyAnalysis,
    string DiscussionHistory
) : BaseEvent(ConversationId, ProductName);
