using MassTransit;
using Prioritizer.Contracts.Commands;
using Prioritizer.Contracts.Events;

namespace Prioritizer.Worker.Workflow;

public class PrioritizationStateMachine : MassTransitStateMachine<PrioritizationState>
{
    public State MarketAnalystTurn { get; private set; } = null!;
    public State TechLeadTurn { get; private set; } = null!;
    public State ProductManagerTurn { get; private set; } = null!;
    public State Completed { get; private set; } = null!;

    public Event<StartPrioritizationCommand> StartPrioritization { get; private set; } = null!;
    public Event<AgentInputProvidedEvent> AgentInputProvided { get; private set; } = null!;
    public Event<PrioritizationDecisionMadeEvent> PrioritizationDecisionMade { get; private set; } = null!;

    public PrioritizationStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => StartPrioritization, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => AgentInputProvided, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => PrioritizationDecisionMade, x => x.CorrelateById(context => context.Message.CorrelationId));

        // Initial state and first turn
        Initially(
            When(StartPrioritization)
                .Then(context =>
                {
                    context.Saga.InitialProposal = context.Message.InitialProposal;
                    context.Saga.TurnCount = 1;
                    context.Saga.DiscussionHistory.Add($"INITIAL PROPOSAL: {context.Message.InitialProposal}");
                })
                .Send(context => new AnalyzeProposalCommand(
                    context.Saga.CorrelationId,
                    "MarketAnalyst",
                    "Critically evaluate this proposal based on market opportunity, competitive landscape, and ROI.",
                    context.Saga.InitialProposal
                ))
                .TransitionTo(MarketAnalystTurn)
        );

        // Market Analyst Turn -> Transition to Tech Lead
        During(MarketAnalystTurn,
            When(AgentInputProvided)
                .Then(context =>
                {
                    context.Saga.DiscussionHistory.Add($"[MARKET ANALYST]: {context.Message.Input}");
                    context.Saga.TurnCount++;
                })
                .Send(context => new AnalyzeProposalCommand(
                    context.Saga.CorrelationId,
                    "TechLead",
                    "Review the market analysis and provide a technical feasibility assessment, highlighting key risks and implementation difficulty.",
                    string.Join("\n", context.Saga.DiscussionHistory)
                ))
                .TransitionTo(TechLeadTurn)
        );

        // Tech Lead Turn -> Transition to Product Manager
        During(TechLeadTurn,
            When(AgentInputProvided)
                .Then(context =>
                {
                    context.Saga.DiscussionHistory.Add($"[TECH LEAD]: {context.Message.Input}");
                    context.Saga.TurnCount++;
                })
                .Send(context => new AnalyzeProposalCommand(
                    context.Saga.CorrelationId,
                    "ProductManager",
                    "Consolidate the market and technical input to provide a FINAL prioritization decision and clear rationale. Format as: 'FEATURE: [name]; RATIONALE: [reason]'.",
                    string.Join("\n", context.Saga.DiscussionHistory)
                ))
                .TransitionTo(ProductManagerTurn)
        );

        // Product Manager Turn -> Finalize
        During(ProductManagerTurn,
            When(PrioritizationDecisionMade)
                .Then(context => context.Saga.DiscussionHistory.Add($"[FINAL DECISION]: {context.Message.FinalDecision}"))
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }
}