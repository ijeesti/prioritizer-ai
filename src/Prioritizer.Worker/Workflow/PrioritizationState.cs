using MassTransit;

namespace Prioritizer.Worker.Workflow;

public class PrioritizationState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = null!;

    public string InitialProposal { get; set; } = null!;
    public List<string> DiscussionHistory { get; set; } = [];
    public int TurnCount { get; set; }
}