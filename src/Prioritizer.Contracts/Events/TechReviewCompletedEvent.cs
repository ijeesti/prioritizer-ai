namespace Prioritizer.Contracts.Events;

public record TechReviewCompletedEvent(
    Guid ConversationId,
    string TechReview,
    string DiscussionHistory,
    string ProductName
) : BaseEvent(ConversationId, ProductName);

