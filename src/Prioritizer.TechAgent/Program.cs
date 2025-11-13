using MassTransit;
using Microsoft.SemanticKernel;
using Prioritizer.Contracts.Events;
using Prioritizer.MarketAgent.Consumers;
using Prioritizer.Shared.MassTransit;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices(ConfigureServices);
var host = builder.Build();
host.Run();

static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    services.AddSingleton(provider =>
    Kernel.CreateBuilder()
        .AddOpenAIChatCompletion("your-model", "access-key")
        .Build());
 
    services
        .AddMassTransitForAgent<TechDecisionConsumer, RequestAnalysisCompletedEvent>("tech-analysis");

}