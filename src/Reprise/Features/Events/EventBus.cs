namespace Reprise
{
    internal sealed class EventBus : IEventBus
    {
        private readonly ILogger<EventBus> _Logger;

        private readonly IServiceScopeFactory _ServiceScopeFactory;

        private readonly IServiceProvider _ServiceProvider;

        private readonly IHostApplicationLifetime _HostApplicationLifetime;

        private readonly TaskRunner _TaskRunner;

        public EventBus(
            ILogger<EventBus> logger,
            IServiceScopeFactory serviceScopeFactory,
            IServiceProvider serviceProvider,
            IHostApplicationLifetime hostApplicationLifetime,
            TaskRunner taskRunner)
        {
            _Logger = logger;
            _ServiceScopeFactory = serviceScopeFactory;
            _ServiceProvider = serviceProvider;
            _HostApplicationLifetime = hostApplicationLifetime;
            _TaskRunner = taskRunner;
        }

        public void Publish(IEvent payload)
        {
            ArgumentNullException.ThrowIfNull(payload);
            var handlerType = typeof(IEventHandler<>).MakeGenericType(payload.GetType());
            var scope = _ServiceScopeFactory.CreateScope();
            var tasks = scope.ServiceProvider.GetServices(handlerType)
                .Select(handler => (Func<Task>)(() => InvokeSafe(handler!, payload)));
            _TaskRunner.WhenAll(tasks)
                .ContinueWith(_ => scope.Dispose());
        }

        public async Task PublishAndWait(IEvent payload, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(payload);
            var handlerType = typeof(IEventHandler<>).MakeGenericType(payload.GetType());
            var tasks = _ServiceProvider.GetServices(handlerType)
                .Select(handler => (Func<Task>)(() => Invoke(handler!, payload, cancellationToken)));
            await _TaskRunner.WhenAll(tasks);
        }

        private async Task InvokeSafe(object handler, IEvent payload)
        {
            var handlerType = handler.GetType();
            try
            {
                var method = handlerType.GetMethod("Handle", new[] { payload.GetType(), typeof(CancellationToken) })!;
                await (Task)method.Invoke(handler, new object[] { payload, _HostApplicationLifetime.ApplicationStopping })!;
            }
            catch (TargetInvocationException targetInvocationEx)
            {
                _Logger.LogEventHandlerException(targetInvocationEx.InnerException!, handlerType.FullName);
            }
            catch (Exception ex)
            {
                _Logger.LogEventHandlerException(ex, handlerType.FullName);
            }
        }

        private static async Task Invoke(object handler, IEvent payload, CancellationToken cancellationToken)
        {
            var handlerType = handler.GetType();
            var method = handlerType.GetMethod("Handle", new[] { payload.GetType(), typeof(CancellationToken) })!;
            try
            {
                await (Task)method.Invoke(handler, new object[] { payload, cancellationToken })!;
            }
            catch (TargetInvocationException targetInvocationEx)
            {
                throw targetInvocationEx.InnerException!;
            }
        }
    }
}
