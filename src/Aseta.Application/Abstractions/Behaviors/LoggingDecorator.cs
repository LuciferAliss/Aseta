using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Primitives.Events;
using Aseta.Domain.Abstractions.Primitives.Results;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Aseta.Application.Abstractions.Behaviors;

internal static class LoggingDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        ILogger<CommandHandler<TCommand, TResponse>> logger)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
        {
            string commandName = typeof(TCommand).Name;

            logger.LogInformation("Processing command {Command}", commandName);

            Result<TResponse> result = await innerHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
            {
                logger.LogInformation("Completed command {Command}", commandName);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    logger.LogError("Completed command {Command} with error", commandName);
                }
            }

            return result;
        }
    }

    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        ILogger<CommandBaseHandler<TCommand>> logger)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
        {
            string commandName = typeof(TCommand).Name;

            logger.LogInformation("Processing command {Command}", commandName);

            Result result = await innerHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
            {
                logger.LogInformation("Completed command {Command}", commandName);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    logger.LogError("Completed command {Command} with error", commandName);
                }
            }

            return result;
        }
    }

    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler,
        ILogger<QueryHandler<TQuery, TResponse>> logger)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken)
        {
            string queryName = typeof(TQuery).Name;

            logger.LogInformation("Processing query {Query}", queryName);

            Result<TResponse> result = await innerHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
            {
                logger.LogInformation("Completed query {Query}", queryName);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    logger.LogError("Completed query {Query} with error", queryName);
                }
            }

            return result;
        }
    }

    internal sealed class DomainEventHandler<TDomainEvent>(
        IDomainEventHandler<TDomainEvent> innerHandler,
        ILogger<DomainEventHandler<TDomainEvent>> logger) 
        : IDomainEventHandler<TDomainEvent> 
        where TDomainEvent : IDomainEvent
    {
        public async Task<Result> Handle(TDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            string domainEventName = typeof(TDomainEvent).Name;

            logger.LogInformation("Processing domain event {DomainEvent}", domainEventName);

            Result result = await innerHandler.Handle(domainEvent, cancellationToken);

            if (result.IsSuccess)
            {
                logger.LogInformation("Completed domain event {DomainEvent}", domainEventName);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    logger.LogError("Completed domain event {DomainEvent} with error", domainEventName);
                }
            }

            return result;
        }
    }
}