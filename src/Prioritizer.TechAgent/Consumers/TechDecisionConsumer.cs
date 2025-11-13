using MassTransit;
using Microsoft.SemanticKernel;
using Prioritizer.Contracts.Events;

namespace Prioritizer.MarketAgent.Consumers;

public class TechDecisionConsumer(Kernel kernel, IPublishEndpoint publishEndpoint) :
    IConsumer<RequestAnalysisCompletedEvent>
{
    public async Task Consume(ConsumeContext<RequestAnalysisCompletedEvent> context)
    {
        Console.WriteLine($"TechDecisionConsumer is Started. ConversationId: {context.Message.ConversationId}");
        var message = context.Message;
        var currentHistory = message.DiscussionHistory + $"\n[POLICY ANALYST]: {message.PolicyAnalysis}";

        var prompt = $"YourDynamicPrompt";//promptService.GetPrompt(..
        var result = await kernel.InvokePromptAsync(prompt);
        var techResponse = result.GetValue<string>()?.Trim() ?? "Technical review failed.";
        var techEvent = new TechReviewCompletedEvent(
            message.ConversationId,
            techResponse, // This is the technical input
            $"{currentHistory} \n[TECH LEAD REVIEW]: {techResponse}",
            message.ProductName
        );

        Console.WriteLine($"TechDecisionConsumer Completed. " +
            $" ConversationId: {context.Message.ConversationId}" +
            $" {techEvent}");

        await publishEndpoint.Publish(techEvent);
    }
}
