using Microsoft.Extensions.Hosting;

namespace Reprise
{
    internal sealed class EventBus : IEventBus
    {
        private readonly ILogger<EventBus> _Logger;

        private readonly IServiceScopeFactory _ServiceScopeFactory;

        private readonly IServiceProvider _ServiceProvider;

        private readonly IHostApplicationLifetime _HostApplicationLifetime;

        public EventBus(ILogger<EventBus> logger, IServiceScopeFactory serviceScopeFactory,
            IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime)
        {
            _Logger = logger;
            _ServiceScopeFactory = serviceScopeFactory;
            _ServiceProvider = serviceProvider;
            _HostApplicationLifetime = hostApplicationLifetime;
        }

        public void Publish(IEvent payload)
        {
            ArgumentNullException.ThrowIfNull(payload);
            var handlerType = typeof(IEventHandler<>).MakeGenericType(payload.GetType());
            var scope = _ServiceScopeFactory.CreateScope();
            var tasks = scope.ServiceProvider.GetServices(handlerType)
                .Select(handler => InvokeSafe(handler!, payload));
            Task.WhenAll(tasks)
                .ContinueWith(_ => scope.Dispose());
        }

        public Task PublishAndWait(IEvent payload, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(payload);
            var handlerType = typeof(IEventHandler<>).MakeGenericType(payload.GetType());
            var tasks = _ServiceProvider.GetServices(handlerType)
                .Select(handler => Invoke(handler!, payload, cancellationToken));
            var waitAllTask = Task.Run(() =>
            {
                try
                {
                    Task.WaitAll(tasks.ToArray(), CancellationToken.None);
                }
                catch (AggregateException ex)
                {
                    throw Flatten(ex);
                }
            }, cancellationToken);

            return waitAllTask;
        }

        private Task InvokeSafe(object handler, IEvent payload)
        {
            return Task.Run(() =>
            {
                var handlerType = handler.GetType();
                try
                {
                    var method = handlerType.GetMethod("Handle", new[] { payload.GetType(), typeof(CancellationToken) })!;
                    var task = (Task)method.Invoke(handler, new object[] { payload, _HostApplicationLifetime.ApplicationStopping })!;
                    task.Wait();
                }
                catch (Exception ex)
                {
                    if (ex is TargetInvocationException or AggregateException)
                    {
                        _Logger.LogEventHandlerException(ex.InnerException!, handlerType.FullName);
                    }
                    else
                    {
                        _Logger.LogEventHandlerException(ex, handlerType.FullName);
                    }
                }
            });
        }

        private static Task Invoke(object handler, IEvent payload, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var handlerType = handler.GetType();
                var method = handlerType.GetMethod("Handle", new[] { payload.GetType(), typeof(CancellationToken) })!;
                var task = (Task)method.Invoke(handler, new object[] { payload, cancellationToken })!;
                task.Wait();
            }, CancellationToken.None);
        }

        private static AggregateException Flatten(AggregateException aggregateException)
        {
            var exceptions = new List<Exception>();
            foreach (var innerException in aggregateException.InnerExceptions)
            {
                if (innerException is TargetInvocationException or AggregateException)
                {
                    exceptions.Add(innerException.InnerException!);
                }
                else
                {
                    exceptions.Add(innerException);
                }
            }

            return new AggregateException(exceptions);
        }
    }
}
