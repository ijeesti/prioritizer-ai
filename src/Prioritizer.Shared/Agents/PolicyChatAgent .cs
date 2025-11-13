using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Prioritizer.Contracts.Models;
using Prioritizer.Shared.AiPlugins;

namespace Prioritizer.Shared.Agents;

/// <summary>
/// Demo -Agent 
/// </summary>
public class PolicyChatAgent(Kernel kernel) : BaseChatAgent(kernel)
{

    public async Task<AiAgentResponse> RunAsync(string instructions, string prompt)
    {
        var agent = Create(instructions);

        var result = await ExecutePromptAsync(agent, prompt);
        //TODO: Parse result and extra exact decision
        return new(result, true);
    }

    protected ChatCompletionAgent Create(string instructions)
        => CreateAgent("PolicyChatAgent ",
            instructions,
            new OpenAIPromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            },            
            typeof(PolicyAgentPlugin));
 
}

