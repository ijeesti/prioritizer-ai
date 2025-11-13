namespace Prioritizer.Contracts.Events;

/// <summary>
/// Provides a shared identifier and timestamp for tracing a single workflow execution.
/// </summary>
public record BaseEvent(
    Guid ConversationId,
    string ProductName,
    DateTimeOffset CreatedAt = default
)
{
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}