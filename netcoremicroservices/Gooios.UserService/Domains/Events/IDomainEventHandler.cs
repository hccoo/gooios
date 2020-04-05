using System.Threading.Tasks;
using Gooios.Infrastructure.Events;

namespace Gooios.UserService.Domain.Events
{
    public interface IDomainEventHandler<TDomainEvent> : IEventHandler<TDomainEvent>
           where TDomainEvent : class, IDomainEvent
    {
    }

    public abstract class BaseDomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
        where TDomainEvent : class, IDomainEvent
    {
        protected bool _isNeedNotification = false;

        protected IEventBus _eventBus;

        protected BaseDomainEventHandler(IEventBus bus,bool isNeedNotification)
        {
            _eventBus = bus;
            _isNeedNotification = (_eventBus != null && isNeedNotification);
        }

        public virtual void Do(TDomainEvent evnt) { }

        public void Handle(TDomainEvent evnt)
        {
            Do(evnt);
            if (_isNeedNotification)
                _eventBus.Publish(evnt);
        }

        public Task HandleAsync(TDomainEvent evnt)
        {
            var task = Task.Run(()=> { Do(evnt); });

            if (_isNeedNotification)
                _eventBus.Publish(evnt);

            return task;
        }
    }
}
