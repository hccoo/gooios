﻿using System;
using System.Threading.Tasks;

namespace Gooios.Infrastructure.Events
{
    internal sealed class ActionDelegatedEventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : IEvent
    {
        #region Private Fields
        private readonly Action<TEvent> eventHandlerDelegate;
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个新的<c>ActionDelegatedDomainEventHandler{TEvent}</c>实例。
        /// </summary>
        /// <param name="eventHandlerDelegate">用于当前领域事件处理器所代理的事件处理委托。</param>
        public ActionDelegatedEventHandler(Action<TEvent> eventHandlerDelegate)
        {
            this.eventHandlerDelegate = eventHandlerDelegate;
        }
        #endregion

        #region IEventHandler<TEvent> Members
        /// <summary>
        /// 处理给定的事件。
        /// </summary>
        /// <param name="evnt">需要处理的事件。</param>
        public void Handle(TEvent evnt)
        {
            Do(evnt);
        }

        #endregion

        /// <summary>
        /// 获取一个<see cref="Boolean"/>值，该值表示当前对象是否与给定的类型相同的另一对象相等。
        /// </summary>
        /// <param name="other">需要比较的与当前对象类型相同的另一对象。</param>
        /// <returns>如果两者相等，则返回true，否则返回false。</returns>
        public override bool Equals(object other)
        {
            if (ReferenceEquals(this, other))
                return true;
            if ((object)other == (object)null)
                return false;
            ActionDelegatedEventHandler<TEvent> otherDelegate = other as ActionDelegatedEventHandler<TEvent>;
            if ((object)otherDelegate == (object)null)
                return false;
            // 使用Delegate.Equals方法判定两个委托是否是代理的同一方法。
            return Delegate.Equals(this.eventHandlerDelegate, otherDelegate.eventHandlerDelegate);
        }

        public override int GetHashCode()
        {
            return this.eventHandlerDelegate.GetHashCode();
        }

        public async Task HandleAsync(TEvent evnt)
        {
            await Task.Run(()=> { Do(evnt); });
        }

        public void Do(TEvent evnt)
        {
            this.eventHandlerDelegate(evnt);
        }
    }
}
