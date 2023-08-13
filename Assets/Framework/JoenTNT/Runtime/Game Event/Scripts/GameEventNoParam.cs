using System;
using System.Collections.Generic;
using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Base class for game event without parameter
    /// </summary>
    [CreateAssetMenu(
        fileName = "New No Param Event",
        menuName = "JT Framework/Game Events/No Param")]
    public class GameEventNoParam : GameEvent
    {
        #region variable

        /// <summary>
        /// Storing unity event
        /// </summary>
        private List<Action> _gameEvent = new List<Action>();

        #endregion

        #region GameEvent

        /// <summary>
        /// Invoke this game event (Send event to all listeners)
        /// </summary>
        public override void Invoke(in object calledBy)
        {
            foreach (var g in _gameEvent)
                g();
#if UNITY_EDITOR
            if (!_invokeLogging) return;

            var stackFrame = new System.Diagnostics.StackFrame(1, true);
            AddLog(calledBy, stackFrame.GetFileName(), stackFrame.GetMethod(), stackFrame.GetFileLineNumber());
#endif
        }

        public override void RemoveAllListeners()
        {
#if UNITY_EDITOR
            _subscribedEvents.Clear();
#endif
            _gameEvent.Clear();
        }
#if UNITY_EDITOR
        protected override void InvokeByPlaceholder() => Invoke(this);
#endif
        #endregion

        #region Main

        /// <summary>
        /// Add listener to game event
        /// </summary>
        /// <param name="action">Action that will be subscribed</param>
        /// <param name="reference">Reference which object registering the event</param>
        public void AddListener(Action action, in object reference = null)
        {
#if UNITY_EDITOR
            _subscribedEvents.Add(new Logging
            {
                reference = reference,
                extender = $"Reference: {reference}\n" +
                    $"Method: {action.Method}\n" +
                    $"Declaring Type: {action.Method.DeclaringType}",
            });
#endif
            _gameEvent.Add(action);
        }

        /// <summary>
        /// Remove listener from game event
        /// </summary>
        /// <param name="action">Action that will be unsubscribed</param>
        /// <param name="reference">Reference which object registering the event</param>
        public void RemoveListener(Action action, in object reference = null)
        {
#if UNITY_EDITOR
            _subscribedEvents.Remove(new Logging
            {
                reference = reference,
                extender = $"Reference: {reference}\n" +
                    $"Method: {action.Method}\n" +
                    $"Declaring Type: {action.Method.DeclaringType}",
            });
#endif
            _gameEvent.Remove(action);
        }

        /// <summary>
        /// Invoke this game event (Send event to all listeners)
        /// </summary>
        public virtual void Invoke() => Invoke(this);

        #endregion
    }
}