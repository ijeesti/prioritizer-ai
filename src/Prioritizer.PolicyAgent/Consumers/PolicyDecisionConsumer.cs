using MassTransit;
using Prioritizer.Contracts.Events;
using Prioritizer.Contracts.Exceptions;
using Prioritizer.PolicyAgent.Policies;
using Prioritizer.Shared.Agents;

namespace Prioritizer.PolicyAgent.Consumers;

public class PolicyDecisionConsumer(PolicyChatAgent policyChatAgent, IPublishEndpoint publishEndpoint) :
    IConsumer<PrioritizationRequestCreatedEvent>
{
    public async Task Consume(ConsumeContext<PrioritizationRequestCreatedEvent> context)
    {
       
        var message = context.Message;
        Console.WriteLine($"PolicyDecisionConsumer is Started. ConversationId: {message.ConversationId}");
        //Add Business logic, logging , validation , db etc.

        // Assume the input contract contains the required product context
        var policyName = message.ProductName.Contains("APIService") ? "PublicAPIService" : "CoreSaaSPlatform";
        var policyText = RuntimePolicies
            .ProductPolicies
            .FirstOrDefault(kvp => kvp.Key == policyName)
            .Value ?? RuntimePolicies.DefaultPolicy;

        var finalHistory = $"Initial Proposal: {message.InitialProposal}";

        string instruction = "Your Instructions";

        // CONSTRUCT THE DYNAMIC EXECUTION PROMPT
        var prompt = $"""
            **YourText**
            {policyText}
            ---
            **DISCUSSION HISTORY:**
            {finalHistory}            
            """;

        var response = await policyChatAgent.RunAsync(instruction, prompt);
        if (!response.Success)
        {
            throw new PolicyNotApprovedException(response.DecisionDetail);
        }

        var analysisCompletedEvent = new RequestAnalysisCompletedEvent(
           ConversationId: message.ConversationId,
           ProductName: message.ProductName,
           InitialProposal: message.InitialProposal,
           PolicyAnalysis: $"Finalized by Policy Manager. [FINAL DECISION]: {response.DecisionDetail}",
           DiscussionHistory: finalHistory
        );
        await publishEndpoint.Publish(analysisCompletedEvent);


        Console.WriteLine($"PolicyDecisionConsumer Done: {analysisCompletedEvent}");
    }
}