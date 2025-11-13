using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Prioritizer.Contracts.Models;
using Prioritizer.Shared.AiPlugins;

namespace Prioritizer.Shared.Agents;

/// <summary>
/// Demo -Agent 
/// </summary>
public class ProductChatAgent(Kernel kernel) : BaseChatAgent(kernel)
{

    public async Task<AiAgentResponse> RunAsync(string instructions, string prompt)
    {
        var agent = Create(instructions);

        var result = await ExecutePromptAsync(agent, prompt);
        //TODO: Parse result and extra exact decision
        return new(result, true);
    }

    protected ChatCompletionAgent Create(string instructions)
        => CreateAgent("ProductChatAgent ",
            instructions,
            new OpenAIPromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
                Temperature = 0.5,
                MaxTokens = 510,
            },
            typeof(ProductChatPlugin));

}
