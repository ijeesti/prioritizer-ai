using MassTransit;
using Microsoft.SemanticKernel;
using Prioritizer.Contracts.Events;
using Prioritizer.Contracts.Interfaces;
using Prioritizer.ProductAgent.Consumers;
using Prioritizer.Shared.Agents;
using Prioritizer.Shared.Emails;
using Prioritizer.Shared.MassTransit;


var builder = Host.CreateDefaultBuilder(args);

// Setup SK
builder.ConfigureServices(ConfigureServices);
var host = builder.Build();
host.Run();

static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    services.AddSingleton<ProductChatAgent>();
    services.AddSingleton(provider =>
    Kernel.CreateBuilder()
             .AddOpenAIChatCompletion("your-model", "access-key")
             .Build());

    services.AddScoped<IEmailService, EmailService>();
    services
        .AddMassTransitForAgent<ProductDecisionConsumer, TechReviewCompletedEvent>("Product-Decision");
}