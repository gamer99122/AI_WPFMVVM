using System;
using System.Collections.Generic;
using System.Linq;

namespace EventAggregatorExample.Services
{
    /// <summary>
    /// 事件聚合器介面
    /// </summary>
    public interface IEventAggregator
    {
        void Subscribe<TMessage>(Action<TMessage> action);
        void Unsubscribe<TMessage>(Action<TMessage> action);
        void Publish<TMessage>(TMessage message);
    }

    /// <summary>
    /// 事件聚合器實作
    /// 實現發布-訂閱模式，用於 ViewModel 之間的通訊
    /// </summary>
    public class EventAggregator : IEventAggregator
    {
        private readonly Dictionary<Type, List<WeakReference>> _subscribers;
        private readonly object _lock = new object();

        public EventAggregator()
        {
            _subscribers = new Dictionary<Type, List<WeakReference>>();
        }

        /// <summary>
        /// 訂閱訊息
        /// </summary>
        public void Subscribe<TMessage>(Action<TMessage> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var messageType = typeof(TMessage);

            lock (_lock)
            {
                if (!_subscribers.ContainsKey(messageType))
                {
                    _subscribers[messageType] = new List<WeakReference>();
                }

                _subscribers[messageType].Add(new WeakReference(action));
            }
        }

        /// <summary>
        /// 取消訂閱
        /// </summary>
        public void Unsubscribe<TMessage>(Action<TMessage> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var messageType = typeof(TMessage);

            lock (_lock)
            {
                if (_subscribers.ContainsKey(messageType))
                {
                    var weakRefs = _subscribers[messageType];
                    weakRefs.RemoveAll(wr => !wr.IsAlive || wr.Target.Equals(action));

                    if (weakRefs.Count == 0)
                    {
                        _subscribers.Remove(messageType);
                    }
                }
            }
        }

        /// <summary>
        /// 發布訊息
        /// </summary>
        public void Publish<TMessage>(TMessage message)
        {
            var messageType = typeof(TMessage);
            List<WeakReference> weakRefs;

            lock (_lock)
            {
                if (!_subscribers.ContainsKey(messageType))
                    return;

                weakRefs = _subscribers[messageType].ToList();
            }

            // 清理已經被回收的訂閱者
            var deadRefs = new List<WeakReference>();

            foreach (var weakRef in weakRefs)
            {
                if (weakRef.IsAlive)
                {
                    var action = weakRef.Target as Action<TMessage>;
                    action?.Invoke(message);
                }
                else
                {
                    deadRefs.Add(weakRef);
                }
            }

            // 移除已死亡的引用
            if (deadRefs.Any())
            {
                lock (_lock)
                {
                    foreach (var deadRef in deadRefs)
                    {
                        _subscribers[messageType].Remove(deadRef);
                    }

                    if (_subscribers[messageType].Count == 0)
                    {
                        _subscribers.Remove(messageType);
                    }
                }
            }
        }
    }

    #region 訊息類別範例

    /// <summary>
    /// 客戶已更新訊息
    /// </summary>
    public class CustomerUpdatedMessage
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
    }

    /// <summary>
    /// 訂單已建立訊息
    /// </summary>
    public class OrderCreatedMessage
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
    }

    /// <summary>
    /// 導航請求訊息
    /// </summary>
    public class NavigationRequestMessage
    {
        public string TargetView { get; set; }
        public object Parameter { get; set; }
    }

    #endregion
}
