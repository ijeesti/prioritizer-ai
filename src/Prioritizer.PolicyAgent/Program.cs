using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Prioritizer.Contracts.Events;
using Prioritizer.PolicyAgent.Consumers;
using Prioritizer.Shared.Agents;
using Prioritizer.Shared.MassTransit;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices(ConfigureServices);
var host = builder.Build();
host.Run();

static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    services.AddSingleton<PolicyChatAgent>();
    services.AddSingleton(provider =>
    Kernel.CreateBuilder()
             .AddOpenAIChatCompletion("your-model", "access-key")
             .Build());

    services
        .AddMassTransitForAgent<PolicyDecisionConsumer, PrioritizationRequestCreatedEvent>("Policy-Queue");
}