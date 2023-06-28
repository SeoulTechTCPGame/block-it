using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[System.Serializable]
public class TypedEvent : UnityEvent<object> { }


public class LogicEventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent> _events;
    private Dictionary<string, TypedEvent> _typedEvents;

    private static LogicEventManager _eventManager;
    public static LogicEventManager instance
    {
        get
        {
            if (!_eventManager)
            {
                _eventManager = FindObjectOfType(typeof(LogicEventManager)) as LogicEventManager;

                if (!_eventManager)
                    Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                else
                    _eventManager.Init();
            }
            return _eventManager;
        }
    }

    void Init()
    {
        if (_events == null)
            _events = new Dictionary<string, UnityEvent>();
        if (_typedEvents == null)
            _typedEvents = new Dictionary<string, TypedEvent>();
    }

    public static void AddListener(string evtName, UnityAction listener)
    {
        UnityEvent evt = null;
        if (instance._events.TryGetValue(evtName, out evt))
        {
            evt.AddListener(listener);
        }
        else
        {
            evt = new UnityEvent();
            evt.AddListener(listener);
            instance._events.Add(evtName, evt);
        }
    }

    public static void RemoveListener(string evtName, UnityAction listener)
    {
        if (_eventManager == null) return;
        UnityEvent evt = null;
        if (instance._events.TryGetValue(evtName, out evt))
            evt.RemoveListener(listener);
    }

    public static void Trigger(string evtName)
    {
        UnityEvent evt = null;
        if (instance._events.TryGetValue(evtName, out evt))
            evt.Invoke();
    }

    public static void AddListener(string evtName, UnityAction<object> listener)
    {
        TypedEvent evt = null;
        if (instance._typedEvents.TryGetValue(evtName, out evt))
        {
            evt.AddListener(listener);
        }
        else
        {
            evt = new TypedEvent();
            evt.AddListener(listener);
            instance._typedEvents.Add(evtName, evt);
        }
    }
    public static void RemoveListener(string evtName, UnityAction<object> listener)
    {
        if (_eventManager == null) return;
        TypedEvent evt = null;
        if (instance._typedEvents.TryGetValue(evtName, out evt))
            evt.RemoveListener(listener);
    }
    public static void Trigger(string evtName, object data)
    {
        TypedEvent evt = null;
        if (instance._typedEvents.TryGetValue(evtName, out evt))
            evt.Invoke(data);
    }
}