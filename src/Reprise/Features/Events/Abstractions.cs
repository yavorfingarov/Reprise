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
        /// Handles an event.
        /// </summary>
        Task Handle(T payload, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Defines an event bus to encapsulate publishing of events.
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Publishes an event without waiting for handlers to finish execution.
        /// </summary>
        /// <remarks>
        /// <para>All matching event handlers are resolved in a new <see cref="IServiceScope"/>, and are executed in parallel.</para>
        /// <para><see cref="IHostApplicationLifetime.ApplicationStopping"/> is passed to the handlers as a cancellation token.</para>
        /// <para>If a handler throws, the exception is logged.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"/>
        void Publish(IEvent payload);

        /// <summary>
        /// Publishes an event and waits for all handlers to finish execution.
        /// </summary>
        /// <remarks>
        /// <para>All matching event handlers are resolved in the current <see cref="IServiceScope"/>, and are executed in parallel.</para>
        /// <para>If one or more handlers throw, all exceptions are packed into an <see cref="AggregateException"/>.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="AggregateException"/>
        Task PublishAndWait(IEvent payload, CancellationToken cancellationToken = default);
    }
}
