//#define SHOW_ERRORS

using System.Collections.Generic;
using UnityEngine;

namespace Magnuth
{
    public class Subscription<T> where T : System.Delegate
    {
        private List<T> _subscribers = null;

// PROPERTIES

        public int Count => _subscribers.Count;

// INITIALISATION

        /// <summary>
        /// Initialises the subscription
        /// </summary>
        public Subscription()
        {
            _subscribers = new List<T>();
        }

        /// <summary>
        /// Initialises the subscription
        /// </summary>
        public Subscription(int capacity)
        {
            _subscribers = new List<T>(capacity);
        }

// SUBSCRIPTIONS

        /// <summary>
        /// Adds the subscriber to the list of subscribers
        /// </summary>
        public void AddSubscriber(T subscriber)
        {
            if (subscriber != null)
            {
                if (!_subscribers.Contains(subscriber))
                {
                    _subscribers.Add(subscriber);

                }
                else ExistingSubscriber(subscriber);
            }
            else NullSubscription();
        }

        /// <summary>
        /// Removes the subscriber from the list of subscribers
        /// </summary>
        public void RemoveSubscriber(T subscriber)
        {
            if (subscriber != null)
            {
                if (_subscribers.Contains(subscriber))
                {
                    _subscribers.Remove(subscriber);

                }
                else MissingSubscriber(subscriber);
            }
            else NullSubscription();
        }

        /// <summary>
        /// Removes all current subscribers from the list of subscribers
        /// </summary>
        public void ClearSubscribers()
        {
            _subscribers.Clear();
        }

        /// <summary>
        /// Notifys all current subscribers
        /// </summary>
        public void NotifySubscribers(params object[] args)
        {
            if (_subscribers.Count == 0) return;

            for (int i = _subscribers.Count - 1; i >= 0; i--)
            {
                _subscribers[i].DynamicInvoke(args);
            }
        }

// ERRORS

        /// <summary>
        /// Logs a null subscriber error to the console
        /// </summary>
        private void NullSubscription()
        {
#if SHOW_ERRORS
            Debug.LogError(new System.NullReferenceException(
                $"Null is an invalid subscriber"
            ));
#endif
        }

        /// <summary>
        /// Logs an existing subscriber error to the console
        /// </summary>
        private void ExistingSubscriber(T action)
        {
#if SHOW_ERRORS
            Debug.LogError(new System.ArgumentException(
                $"{action.Method.Name} is already a current subscriber"
            ));
#endif
        }

        /// <summary>
        /// Logs a missing subscriber error to the console
        /// </summary>
        private void MissingSubscriber(T action)
        {
#if SHOW_ERRORS
            Debug.LogError(new System.InvalidOperationException(
                $"{action.Method.Name} is not a current subscriber"
            ));
#endif
        }
    }
}