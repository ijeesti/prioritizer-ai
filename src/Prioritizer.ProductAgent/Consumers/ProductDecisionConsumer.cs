using MassTransit;
using Microsoft.SemanticKernel;
using Prioritizer.Contracts.Events;
using Prioritizer.Contracts.Interfaces;

namespace Prioritizer.ProductAgent.Consumers;

public class ProductDecisionConsumer(
    Kernel kernel, 
    IPublishEndpoint publishEndpoint,
    IEmailService emailService) : IConsumer<TechReviewCompletedEvent>
{

    public async Task Consume(ConsumeContext<TechReviewCompletedEvent> context)
    {
        Console.WriteLine($"ProductDecisionConsumer is Started. ConversationId: {context.Message.ConversationId}");
        Thread.Sleep(4000);
        var message = context.Message;
        var finalHistory = message.DiscussionHistory + $"\n[TECH LEAD]: {message.TechReview}";
        var prompt = $"**ROLE: Product Manager.** Given the history:\n{finalHistory}\nProvide the final decision (Feature and Rationale).";

        var finalResponse = "Approved for Q1 Development Sprint.\r\nPrioritization Score (RICE/WSJF)\tHigh (Score: 85). " +
            "High Impact (15% conversion lift) / Low Effort (12 days) ratio." +
            "\n Focus on returning logged-in users initially. Guest checkout can be a follow-up feature (Phase 2)";
        //var result = await kernel.InvokePromptAsync(prompt);
        // var finalResponse = result.GetValue<string>()?.Trim() ?? "Decision failed.";


        //add logic to read request detail or add update contract
        //await emailService.SendDecisionEmailAsync(finalResponse, prompt, finalHistory);

        var finalEvent = new PrioritizationDecisionMadeEvent(
            ConversationId: message.ConversationId,
            ProductName: message.ProductName,
            FinalDecision: finalResponse,
            Rationale: "test",
            FullHistory:
            $"Finalized by Product Manager. {finalHistory} \n + [FINAL DECISION]: \n{finalResponse}"
        );

        Console.WriteLine($"ProductDecisionConsumer Completed. " +
            $" ConversationId: {context.Message.ConversationId}" +
            $" {finalEvent}");

        //publish decision
        await publishEndpoint.Publish(finalEvent);
    }
}