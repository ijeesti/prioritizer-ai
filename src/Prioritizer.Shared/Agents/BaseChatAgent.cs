using MassTransit.Internals;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Prioritizer.Shared.Agents;

public abstract class BaseChatAgent(Kernel kernel)
{
    protected ChatCompletionAgent CreateAgent(
        string name, 
        string instructions,
        OpenAIPromptExecutionSettings settings, params Type[] pluginTypes)
    {

        var agent = new ChatCompletionAgent
        {
            Name = name,
            Instructions = instructions,
            Kernel = kernel,
            Arguments = new KernelArguments(settings)
        };

        foreach (var type in pluginTypes)
        {
            var pluginName = type.Name;

            // Check if already registered
            if (!agent.Kernel.Plugins.Contains(pluginName))
            {
                var plugin = KernelPluginFactory.CreateFromType(type);
                agent.Kernel.Plugins.Add(plugin);
            }
        }

        return agent;
    }


    protected async Task<string> ExecutePromptAsync(ChatCompletionAgent agent, string? prompt)
    {
        //Mock 
        return "";

        if (string.IsNullOrWhiteSpace(prompt))
        {
            return string.Empty;
        }

        var replies = await agent.InvokeAsync(prompt).ToListAsync();

        var resultText = string.Concat(
            replies
                .Select(r => r.Message?.Content?.ToString())
                .Where(s => !string.IsNullOrWhiteSpace(s))
        ).Trim();

        return resultText;
    }
}

