using System.Collections.Generic;
using UnityEngine;

namespace JT.GameEvents
{
    /// <summary>
    /// Base class for game event
    /// </summary>
    [System.Serializable]
    public abstract class GameEvent : ScriptableObject
    {
#if UNITY_EDITOR
        #region struct

        [System.Serializable]
        public struct Logging
        {
            public object reference;
            public string extender;
        }

        #endregion

        #region Variable

        [SerializeField]
        protected bool _invokeLogging = false;

        [SerializeField]
        private int _limitLoggingCount = 1;

        [SerializeField]
        private List<Logging> _logs = new List<Logging>();

        [SerializeField]
        protected List<Logging> _subscribedEvents = new List<Logging>();

        #endregion
#endif
        #region Main

        /// <summary>
        /// Remove all listeners from game event
        /// </summary>
        public abstract void RemoveAllListeners();

        /// <summary>
        /// Invoke game event with default values.
        /// </summary>
        /// <param name="calledBy">Origin object who called this event.</param>
        public abstract void Invoke(in object calledBy);

#if UNITY_EDITOR
        /// <summary>
        /// Called in editor to invoke game event.
        /// </summary>
        protected abstract void InvokeByPlaceholder();

        /// <summary>
        /// Add log message when calling a game event, run it manually after invoke.
        /// </summary>
        /// <param name="callReference">The object who call the invoke method</param>
        /// <param name="fileName">Script who invoke the method</param>
        /// <param name="method">Method reference where the event being called</param>
        /// <param name="atLine">At script line number</param>
        protected void AddLog(object callReference, string fileName, System.Reflection.MethodBase method, int atLine)
        {
            if (!_invokeLogging) return;
            _logs.Add(new Logging()
            {
                reference = callReference,
                extender = $"Called By: {callReference}\n" +
                $"On Script: {fileName}\n" +
                $"On Method: {method}" +
                $"Called At Line: {atLine}"
            });

            if (_logs.Count <= _limitLoggingCount) return;
            _logs.RemoveAt(0);
        }
#endif
        #endregion
    }

    /// <summary>
    /// Base class for game event with 1 parameter
    /// </summary>
    public abstract class GameEvent<T> : GameEvent
    {
        #region Variable

        /// <summary>
        /// Storing unity event
        /// </summary>
        private List<System.Action<T>> _gameEvent = new List<System.Action<T>>();

        /// <summary>
        /// Default first parameter value of the game event.
        /// </summary>
        [SerializeField]
        private T _defaultParam1 = default;
#if UNITY_EDITOR
        /// <summary>
        /// Placeholder variable used in Editor to call an event.
        /// </summary>
        [SerializeField]
        private T _param1Placeholder = default;
#endif
        #endregion

        #region GameEvent

        public override void Invoke(in object calledBy) => Invoke(_defaultParam1, calledBy);

        public override void RemoveAllListeners()
        {
#if UNITY_EDITOR
            _subscribedEvents.Clear();
#endif
            _gameEvent.Clear();
        }
#if UNITY_EDITOR
        protected override void InvokeByPlaceholder() => Invoke(_param1Placeholder, this);
#endif
        #endregion

        #region Main

        /// <summary>
        /// Add listener to game event
        /// </summary>
        /// <param name="action">Action that will be subscribed</param>
        /// <param name="reference">Reference which object registering the event</param>
        public void AddListener(System.Action<T> action, in object reference = null)
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
        public void RemoveListener(System.Action<T> action, in object reference = null)
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
        /// <param name="p1">1st parameter</param>
        public virtual void Invoke(T p1) => Invoke(p1, null);

        /// <summary>
        /// Invoke this game event (Send event to all listeners)
        /// </summary>
        /// <param name="p1">1st parameter</param>
        /// <param name="calledBy">Origin object who called this event.</param>
        public virtual void Invoke(T p1, in object calledBy)
        {
            foreach (var g in _gameEvent)
                g(p1);
#if UNITY_EDITOR
            if (!_invokeLogging) return;

            var stackFrame = new System.Diagnostics.StackFrame(1, true);
            AddLog(calledBy, stackFrame.GetFileName(), stackFrame.GetMethod(), stackFrame.GetFileLineNumber());
#endif
        }

        #endregion
    }

    /// <summary>
    /// Base class for game event with 2 parameters
    /// </summary>
    public abstract class GameEvent<T1, T2> : GameEvent
    {
        #region Variable

        /// <summary>
        /// Storing unity event
        /// </summary>
        private List<System.Action<T1, T2>> _gameEvent = new List<System.Action<T1, T2>>();

        /// <summary>
        /// Default first parameter value of the game event.
        /// </summary>
        [SerializeField]
        private T1 _defaultParam1 = default;

        /// <summary>
        /// Default second parameter value of the game event.
        /// </summary>
        [SerializeField]
        private T2 _defaultParam2 = default;
#if UNITY_EDITOR
        /// <summary>
        /// First parameter placeholder variable used in Editor to call an event.
        /// </summary>
        [SerializeField]
        private T1 _param1Placeholder = default;

        /// <summary>
        /// Second parameter placeholder variable used in Editor to call an event.
        /// </summary>
        [SerializeField]
        private T2 _param2Placeholder = default;
#endif
        #endregion

        #region GameEvent

        public override void Invoke(in object calledBy) => Invoke(_defaultParam1, _defaultParam2, calledBy);

        public override void RemoveAllListeners()
        {
#if UNITY_EDITOR
            _subscribedEvents.Clear();
#endif
            _gameEvent.Clear();
        }
#if UNITY_EDITOR
        protected override void InvokeByPlaceholder() => Invoke(_param1Placeholder, _param2Placeholder, this);
#endif
        #endregion

        #region Main

        /// <summary>
        /// Add listener to game event
        /// </summary>
        /// <param name="action">Action that will be subscribed</param>
        /// <param name="reference">Reference which object registering the event</param>
        public void AddListener(System.Action<T1, T2> action, in object reference = null)
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
        public void RemoveListener(System.Action<T1, T2> action, in object reference = null)
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
        /// Invoke this game event(Send event to all listeners)
        /// </summary>
        /// <param name="p1">1st parameter</param>
        /// <param name="p2">2nd parameter</param>
        public void Invoke(T1 p1, T2 p2) => Invoke(p1, p2, null);

        /// <summary>
        /// Invoke this game event(Send event to all listeners)
        /// </summary>
        /// <param name="p1">1st parameter</param>
        /// <param name="p2">2nd parameter</param>
        /// <param name="calledBy">Origin object who called this event.</param>
        public void Invoke(T1 p1, T2 p2, in object calledBy)
        {
            foreach (var g in _gameEvent)
                g(p1, p2);
#if UNITY_EDITOR
            if (!_invokeLogging) return;

            var stackFrame = new System.Diagnostics.StackFrame(1, true);
            AddLog(calledBy, stackFrame.GetFileName(), stackFrame.GetMethod(), stackFrame.GetFileLineNumber());
#endif
        }

        #endregion
    }

    /// <summary>
    /// Base class for game event with 3 parameters
    /// </summary>
    public abstract class GameEvent<T1, T2, T3> : GameEvent
    {
        #region Variable

        /// <summary>
        /// Storing unity event
        /// </summary>
        private List<System.Action<T1, T2, T3>> _gameEvent = new List<System.Action<T1, T2, T3>>();

        /// <summary>
        /// Default first parameter value of the game event.
        /// </summary>
        [SerializeField]
        private T1 _defaultParam1 = default;

        /// <summary>
        /// Default second parameter value of the game event.
        /// </summary>
        [SerializeField]
        private T2 _defaultParam2 = default;

        /// <summary>
        /// Default third parameter value of the game event.
        /// </summary>
        [SerializeField]
        private T3 _defaultParam3 = default;
#if UNITY_EDITOR
        /// <summary>
        /// First parameter placeholder variable used in Editor to call an event.
        /// </summary>
        [SerializeField]
        private T1 _param1Placeholder = default;

        /// <summary>
        /// Second parameter placeholder variable used in Editor to call an event.
        /// </summary>
        [SerializeField]
        private T2 _param2Placeholder = default;

        /// <summary>
        /// Third parameter placeholder variable used in Editor to call an event.
        /// </summary>
        [SerializeField]
        private T3 _param3Placeholder = default;
#endif
        #endregion

        #region GameEvent

        public override void Invoke(in object calledBy) => Invoke(_defaultParam1, _defaultParam2, _defaultParam3, calledBy);

        public override void RemoveAllListeners()
        {
#if UNITY_EDITOR
            _subscribedEvents.Clear();
#endif
            _gameEvent.Clear();
        }
#if UNITY_EDITOR
        protected override void InvokeByPlaceholder() => Invoke(_param1Placeholder, _param2Placeholder, _param3Placeholder, this);
#endif
        #endregion

        #region Main

        /// <summary>
        /// Add listener to game event
        /// </summary>
        /// <param name="action">Action that will be subscribed</param>
        /// <param name="reference">Reference which object registering the event</param>
        public void AddListener(System.Action<T1, T2, T3> action, in object reference = null)
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
        public void RemoveListener(System.Action<T1, T2, T3> action, in object reference = null)
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
        /// Invoke this game event(Send event to all listeners)
        /// </summary>
        /// <param name="p1">1st parameter</param>
        /// <param name="p3">2nd parameter</param>
        /// <param name="p3">3rd parameter</param>
        public void Invoke(T1 p1, T2 p2, T3 p3) => Invoke(p1, p2, p3, null);

        /// <summary>
        /// Invoke this game event(Send event to all listeners)
        /// </summary>
        /// <param name="p1">1st parameter</param>
        /// <param name="p3">2nd parameter</param>
        /// <param name="p3">3rd parameter</param>
        /// <param name="calledBy">Origin object who called this event.</param>
        public void Invoke(T1 p1, T2 p2, T3 p3, in object calledBy)
        {
            foreach (var g in _gameEvent)
                g(p1, p2, p3);
#if UNITY_EDITOR
            if (!_invokeLogging) return;

            var stackFrame = new System.Diagnostics.StackFrame(1, true);
            AddLog(calledBy, stackFrame.GetFileName(), stackFrame.GetMethod(), stackFrame.GetFileLineNumber());
#endif
        }

        #endregion
    }


    /// <summary>
    /// Base class for game event with 3 parameters
    /// </summary>
    public abstract class GameEvent<T1, T2, T3, T4> : GameEvent
    {
        #region Variable

        /// <summary>
        /// Storing unity event
        /// </summary>
        private List<System.Action<T1, T2, T3, T4>> _gameEvent = new List<System.Action<T1, T2, T3, T4>>();

        /// <summary>
        /// Default first parameter value of the game event.
        /// </summary>
        [SerializeField]
        private T1 _defaultParam1 = default;

        /// <summary>
        /// Default second parameter value of the game event.
        /// </summary>
        [SerializeField]
        private T2 _defaultParam2 = default;

        /// <summary>
        /// Default third parameter value of the game event.
        /// </summary>
        [SerializeField]
        private T3 _defaultParam3 = default;

        /// <summary>
        /// Default fourth parameter value of the game event.
        /// </summary>
        [SerializeField]
        private T4 _defaultParam4 = default;
#if UNITY_EDITOR
        /// <summary>
        /// First parameter placeholder variable used in Editor to call an event.
        /// </summary>
        [SerializeField]
        private T1 _param1Placeholder = default;

        /// <summary>
        /// Second parameter placeholder variable used in Editor to call an event.
        /// </summary>
        [SerializeField]
        private T2 _param2Placeholder = default;

        /// <summary>
        /// Third parameter placeholder variable used in Editor to call an event.
        /// </summary>
        [SerializeField]
        private T3 _param3Placeholder = default;

        /// <summary>
        /// Fourth parameter placeholder variable used in Editor to call an event.
        /// </summary>
        [SerializeField]
        private T4 _param4Placeholder = default;
#endif
        #endregion

        #region GameEvent

        public override void Invoke(in object calledBy) => Invoke(_defaultParam1, _defaultParam2, _defaultParam3, _defaultParam4, calledBy);

        public override void RemoveAllListeners()
        {
#if UNITY_EDITOR
            _subscribedEvents.Clear();
#endif
            _gameEvent.Clear();
        }
#if UNITY_EDITOR
        protected override void InvokeByPlaceholder() => Invoke(_param1Placeholder, _param2Placeholder, _param3Placeholder, _param4Placeholder, this);
#endif
        #endregion

        #region Main

        /// <summary>
        /// Add listener to game event
        /// </summary>
        /// <param name="action">Action that will be subscribed</param>
        /// <param name="reference">Reference which object registering the event</param>
        public void AddListener(System.Action<T1, T2, T3, T4> action, in object reference = null)
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
        public void RemoveListener(System.Action<T1, T2, T3, T4> action, in object reference = null)
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
        /// Invoke this game event(Send event to all listeners)
        /// </summary>
        /// <param name="p1">1st parameter</param>
        /// <param name="p3">2nd parameter</param>
        /// <param name="p3">3rd parameter</param>
        /// <param name="p3">4th parameter</param>
        public void Invoke(T1 p1, T2 p2, T3 p3, T4 p4) => Invoke(p1, p2, p3, p4, null);

        /// <summary>
        /// Invoke this game event(Send event to all listeners)
        /// </summary>
        /// <param name="p1">1st parameter</param>
        /// <param name="p3">2nd parameter</param>
        /// <param name="p3">3rd parameter</param>
        /// <param name="p3">4th parameter</param>
        /// <param name="calledBy">Origin object who called this event.</param>
        public void Invoke(T1 p1, T2 p2, T3 p3, T4 p4, in object calledBy)
        {
            foreach (var g in _gameEvent)
                g(p1, p2, p3, p4);
#if UNITY_EDITOR
            if (!_invokeLogging) return;

            var stackFrame = new System.Diagnostics.StackFrame(1, true);
            AddLog(calledBy, stackFrame.GetFileName(), stackFrame.GetMethod(), stackFrame.GetFileLineNumber());
#endif
        }

        #endregion
    }
}