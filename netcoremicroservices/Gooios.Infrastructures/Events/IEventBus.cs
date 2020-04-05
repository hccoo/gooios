using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Gooios.Infrastructure.Events
{
    public interface IEventBus:IUnitOfWork, IDisposable
    {
        Guid ID { get; }
        /// <summary>
        /// Publishes the specified message to the bus.
        /// </summary>
        /// <param name="message">The message to be published.</param>
        void Publish<TMessage>(TMessage message)
            where TMessage : class, IEvent;
        /// <summary>
        /// Publishes a collection of messages to the bus.
        /// </summary>
        /// <param name="messages">The messages to be published.</param>
        void Publish<TMessage>(IEnumerable<TMessage> messages)
            where TMessage : class, IEvent;

        /// <summary>
        /// Clears the published messages waiting for commit.
        /// </summary>
        void Clear();
    }

    public class EventBus : DisposableObject, IEventBus
    {
        private readonly Guid id = Guid.NewGuid();
        private readonly ThreadLocal<Queue<object>> _messageQueue = new ThreadLocal<Queue<object>>(() => new Queue<object>());
        private readonly IMediator _aggregator;
        private readonly ThreadLocal<bool> _committed = new ThreadLocal<bool>(() => true);
        private readonly MethodInfo _publishMethod;

        public EventBus(IMediator aggregator)
        {
            _aggregator = aggregator;
            _publishMethod = (from m in aggregator.GetType().GetMethods()
                             let parameters = m.GetParameters()
                             let methodName = m.Name
                             where methodName == "Publish" &&
                             parameters != null
                             select m).First();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _messageQueue.Dispose();
                _committed.Dispose();
            }
        }

        #region IBus Members

        public void Publish<TMessage>(TMessage message)
            where TMessage : class, IEvent
        {
            _messageQueue.Value.Enqueue(message);
            _committed.Value = false;
        }

        public void Publish<TMessage>(IEnumerable<TMessage> messages)
            where TMessage : class, IEvent
        {
            foreach (var message in messages)
                Publish(message);
        }

        public void Clear()
        {
            _messageQueue.Value.Clear();
            _committed.Value = true;
        }

        #endregion

        #region IUnitOfWork Members

        public bool DistributedTransactionSupported
        {
            get { return false; }
        }

        public bool Committed
        {
            get { return _committed.Value; }
        }

        public void Commit()
        {
            while (_messageQueue.Value.Count > 0)
            {
                var evnt = _messageQueue.Value.Dequeue();
                var evntType = evnt.GetType();
                var method = _publishMethod.MakeGenericMethod(evntType);
                method.Invoke(_aggregator, new object[] { evnt,null });
            }
            _committed.Value = true;
        }

        public void Rollback()
        {
            Clear();
        }

        public Guid ID
        {
            get { return id; }
        }

        #endregion
    }

    public interface ITest : IUnitOfWork, IDisposable
    {

    }

    public class Test : DisposableObject, ITest
    {
        public void Commit()
        {
            
        }

        protected override void Dispose(bool disposing)
        {
            
        }
    }
}
