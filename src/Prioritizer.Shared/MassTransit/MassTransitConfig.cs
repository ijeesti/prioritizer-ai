using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Prioritizer.Shared.MassTransit;

public static class MassTransitConfig
{
    public static void ConfigureAgentEndpoint<TConsumer, TMessage>(
        IRabbitMqBusFactoryConfigurator cfg,
        IBusRegistrationContext context,
        string agentRoleName)
        where TConsumer : class, IConsumer<TMessage>
        where TMessage : class
    {
        string queueName = $"Prioritization.{agentRoleName}";
        string dlxName = $"Prioritization.{agentRoleName}-Dead";

        // 1. Declare the dead-letter queue first
        cfg.ReceiveEndpoint(dlxName, e =>
        {
            e.Durable = true;
            e.AutoDelete = false;
            e.SetQuorumQueue();
        });

        // 2. Declare the main queue pointing to the DLQ
        cfg.ReceiveEndpoint(queueName, e =>
        {
            e.Durable = true;
            e.AutoDelete = false;
            e.SetQuorumQueue();

            if (e is IRabbitMqReceiveEndpointConfigurator rabbitMqConfigurator)
            {
                rabbitMqConfigurator.DeadLetterExchange = dlxName;
            }

            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(10)));
            e.ConfigureConsumer<TConsumer>(context);
        });
    }

    public static void AddMassTransitForAgent<TConsumer, TMessage>(this IServiceCollection services, string agentRoleName)
        where TConsumer : class, IConsumer<TMessage>
        where TMessage : class
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<TConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                ConfigureAgentEndpoint<TConsumer, TMessage>(cfg, context, agentRoleName);
            });
        });
    }
}

//public static class MassTransitConfig
//{
//    public static void ConfigureAgentEndpoint<TConsumer, TMessage>(
//        IRabbitMqBusFactoryConfigurator cfg,
//        IBusRegistrationContext context,
//        string agentRoleName)
//        where TConsumer : class, IConsumer<TMessage>
//        where TMessage : class
//    {
//        string queueName = $"Prioritization.{agentRoleName}";
//        string dlxName = $"Prioritization.{agentRoleName}-Dead";


//        cfg.ReceiveEndpoint(dlxName, e =>
//        {
//            e.Durable = true;
//            e.AutoDelete = false;
//            e.SetQuorumQueue();
//        });

//        cfg.ReceiveEndpoint(queueName, e =>
//        {
//            if (e is IRabbitMqReceiveEndpointConfigurator rabbitMqConfigurator)
//            {
//                rabbitMqConfigurator.Durable = true;
//                rabbitMqConfigurator.DeadLetterExchange = dlxName;
//            }

//            e.SetQuorumQueue();
//            //e.PrefetchCount = 20;
//            e.UseMessageRetry(r =>
//                r.Interval(3, TimeSpan.FromSeconds(10))
//            );
//            e.ConfigureConsumer<TConsumer>(context);
//        });
//    }

//    public static void AddMassTransitForAgent<TConsumer, TMessage>(this IServiceCollection services, string agentRoleName)
//        where TConsumer : class, IConsumer<TMessage>
//        where TMessage : class
//    {
//        services.AddMassTransit(x =>
//        {
//            x.AddConsumer<TConsumer>();

//            x.UsingRabbitMq((context, cfg) =>
//            {
//                cfg.Host("localhost", "/", h =>
//                {
//                    h.Username("guest");
//                    h.Password("guest");
//                });
//                ConfigureAgentEndpoint<TConsumer, TMessage>(cfg, context, agentRoleName);
//            });
//        });
//    }
//}