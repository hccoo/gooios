using Gooios.Infrastructure.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.UserService.Domain.Events
{
    public interface IDomainEvent : IEvent
    {
        IEntity Source { get; }
    }

    [Serializable]
    public abstract class DomainEvent : IDomainEvent
    {
        #region Private Fields
        private readonly IEntity source;
        private Guid id = Guid.NewGuid();
        private DateTime timeStamp = DateTime.UtcNow;
        #endregion

        #region Ctor
        public DomainEvent() { }
        public DomainEvent(IEntity source)
        {
            this.source = source;
        }
        #endregion

        #region IDomainEvent Members
        /// <summary>
        /// 获取领域事件的全局唯一标识。
        /// </summary>
        public Guid ID
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 获取产生领域事件的时间戳。
        /// </summary>
        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

        public IEntity Source
        {
            get { return source; }
        }
        #endregion

        #region Public Static Methods

        public static void Publish<TDomainEvent>(TDomainEvent domainEvent)
            where TDomainEvent : class, IDomainEvent
        {
            using (var scope = IocProvider.Container.CreateScope())
            {
                IEnumerable<IDomainEventHandler<TDomainEvent>> handlers =scope.ServiceProvider.GetServices<IDomainEventHandler<TDomainEvent>>();// ServiceLocator.Instance.ResolveAll<IDomainEventHandler<TDomainEvent>>();
                foreach (var handler in handlers)
                {
                    if (handler.GetType().IsDefined(typeof(HandlesAsynchronouslyAttribute), false))
                        handler.HandleAsync(domainEvent);
                    else
                        handler.Handle(domainEvent);
                }
            }
        }

        public static void Publish<TDomainEvent>(TDomainEvent domainEvent, Action<TDomainEvent, bool, Exception> callback)
            where TDomainEvent : class, IDomainEvent
        {
            using (var scope = IocProvider.Container.CreateScope())
            {
                IEnumerable<IDomainEventHandler<TDomainEvent>> handlers = scope.ServiceProvider.GetServices<IDomainEventHandler<TDomainEvent>>(); //ServiceLocator.Instance.ResolveAll<IDomainEventHandler<TDomainEvent>>();
                if (handlers != null && handlers.Any())
                {
                    try
                    {
                        foreach (var handler in handlers)
                        {
                            if (handler.GetType().IsDefined(typeof(HandlesAsynchronouslyAttribute), false))
                                handler.HandleAsync(domainEvent);
                            else
                                handler.Handle(domainEvent);
                        }
                        callback(domainEvent, true, null);
                    }
                    catch (Exception ex)
                    {
                        callback(domainEvent, false, ex);
                    }
                }
                else
                    callback(domainEvent, false, null);
            }
        }
        #endregion
    }
}
