using MassTransit;
using Prioritizer.API.Services;
using Prioritizer.Contracts.Events;
using Prioritizer.Contracts.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer(); // Required for Swagger to discover endpoints
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

builder.Services.AddScoped<PrioritizationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/prioritize", async (PrioritizeRequest request, PrioritizationService bus) =>
{
    var conversationId = Guid.NewGuid();
    Console.WriteLine($"conversationId {conversationId} is published");
    await bus.SendMessage(new PrioritizationRequestCreatedEvent(conversationId, request.ProductName, request.Proposal));

    var response = new PrioritizeResponse(
        ConversationId: conversationId,
        Message: $"/status/{conversationId}",
        Success: true
    );

    return Results.Accepted($"/status/{conversationId}", response);
})
.WithName("StartPrioritization")
.WithSummary("Initiates a new multi-agent prioritization discussion.")
.WithDescription("Sends an initial proposal to the message broker, starting an asynchronous workflow across various agent services.")
.Produces<PrioritizeResponse>(StatusCodes.Status202Accepted)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status500InternalServerError);

app.Run();
