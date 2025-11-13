using MassTransit;
using Prioritizer.Contracts.Events;

namespace Prioritizer.API.Services;

//Demo Class..
public class PrioritizationService(ISendEndpointProvider sendEndpointProvider)
{

    public async Task SendMessage(PrioritizationRequestCreatedEvent message)
    {
        var endpoint = await sendEndpointProvider.GetSendEndpoint(
            new Uri("rabbitmq://localhost/Prioritization.Policy-Queue"));
        await endpoint.Send(message);
    }
}