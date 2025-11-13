namespace Prioritizer.Contracts.Models;

/// <summary>
/// Response returned upon successful initiation of the asynchronous prioritization process.
/// </summary>
public record PrioritizeResponse(
    Guid ConversationId,
    string Message,
    bool Success
);