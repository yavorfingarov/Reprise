using Microsoft.Extensions.Hosting;

namespace Reprise
{
    /// <summary>
    /// Marker interface to represent an event.
    /// </summary>
    public interface IEvent
    {
    }

    /// <summary>
    /// Specifies the contract to handle events.
    /// </summary>
    public interface IEventHandler<T> where T : IEvent
    {
        /// <summary>
        /// Handles the <paramref name="payload"/>. <see cref="IHostApplicationLifetime.ApplicationStopping"/> is passed 
        /// as <paramref name="stoppingToken"/>.
        /// </summary>
        Task Handle(T payload, CancellationToken stoppingToken);
    }

    /// <summary>
    /// Defines an event bus to encapsulate publishing of events.
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Publishes an event.
        /// </summary>
        /// <remarks>
        /// The method returns immediately. Under the hood, a new <see cref="IServiceScope"/> is created, 
        /// all matching event handlers are resolved, and are executed in parallel. 
        /// <para>If a handler throws, the exception is logged.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"/>
        void Publish(IEvent payload);
    }

    internal sealed class EventBus : IEventBus
    {
        private readonly ILogger _Logger;

        private readonly IServiceScopeFactory _ServiceScopeFactory;

        private readonly CancellationToken _StoppingToken;

        public EventBus(ILoggerFactory loggerFactory, IServiceScopeFactory serviceScopeFactory,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            _Logger = loggerFactory.CreateLogger(GetType().FullName!);
            _ServiceScopeFactory = serviceScopeFactory;
            _StoppingToken = hostApplicationLifetime.ApplicationStopping;
        }

        public void Publish(IEvent payload)
        {
            ArgumentNullException.ThrowIfNull(payload);
            var handlerType = typeof(IEventHandler<>).MakeGenericType(payload.GetType());
            var scope = _ServiceScopeFactory.CreateScope();
            var tasks = scope.ServiceProvider.GetServices(handlerType)
                .Select(handler => Invoke(handler!, payload));
            Task.WhenAll(tasks)
                .ContinueWith(_ => scope.Dispose());
        }

        private Task Invoke(object handler, IEvent payload)
        {
            return Task.Run(() =>
            {
                var handlerType = handler.GetType();
                try
                {
                    var method = handlerType.GetMethod("Handle")!;
                    var task = (Task)method.Invoke(handler, new object[] { payload, _StoppingToken })!;
                    task.Wait();
                }
                catch (TargetInvocationException ex)
                {
                    _Logger.LogEventHandlerException(ex.InnerException!, handlerType.FullName);
                }
                catch (AggregateException ex)
                {
                    _Logger.LogEventHandlerException(ex.InnerException!, handlerType.FullName);
                }
                catch (Exception ex)
                {
                    _Logger.LogEventHandlerException(ex, handlerType.FullName);
                }
            });
        }
    }

    internal static partial class LoggerExtensions
    {
        private static readonly Action<ILogger, string?, Exception> _MessageHandlerException = LoggerMessage.Define<string?>(
            LogLevel.Error, default,
            "An exception was thrown while executing {EventHandlerType}.");

        public static void LogEventHandlerException(this ILogger logger, Exception exception, string? eventHandlerType)
        {
            _MessageHandlerException(logger, eventHandlerType, exception);
        }
    }

    internal sealed class EventHandlerTypeProcessor : AbstractTypeProcessor
    {
        internal override void Process(WebApplicationBuilder builder, Type type)
        {
            var interfaceTypes = type.GetInterfaces();
            foreach (var interfaceType in interfaceTypes)
            {
                if (interfaceType.IsGenericType)
                {
                    var genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(IEventHandler<>))
                    {
                        builder.Services.AddScoped(interfaceType, type);

                        break;
                    }
                }
            }
        }

        internal override void PostProcess(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IEventBus, EventBus>();
        }
    }
}
