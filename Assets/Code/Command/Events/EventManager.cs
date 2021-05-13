﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///     Manages global events for application flow. 
///     NW: HSW-4073 - Reduced GC by:
///         1. Made EventManager generic, reducing the conversion to Enum. 
///         2. Made EventManager pooled param overloads to prevent object array creation. 
///         3. Converted Action invocation list to List{Action} to save GC allocs on Count query.  
///         4. Added a list pooler for point 3 (PoolFactory.cs).  
/// </summary>
/// <typeparam name="T">Event type (enum).</typeparam>
public class EventManager<T> : AutoGeneratedSingleton<EventManager<T>>
    where T : struct
{
    private readonly PoolFactory<List<Action<T, object[]>>> m_listenerPool;
    private readonly Dictionary<T, List<Action<T, object[]>>> m_events;

    public EventManager(int expectedEventTypes, int expectedRegisterCalls)
    {
        // NW: Doing this because we cannot call the correct consturctor ourselves (AutoGeneratedSingleton!!).
        // The Events (enum) EventManager is huge. Allocate here, ourselves, with massive capacity.
        // Optimization / hack. 
        if (typeof(T).FullName == "Events")
        {
            expectedEventTypes = 70;
            expectedRegisterCalls = 80;
        }

        m_listenerPool = new PoolFactory<List<Action<T, object[]>>>(
                () => new List<Action<T, object[]>>(2), x => x.Clear(), expectedRegisterCalls);
        m_events = new Dictionary<T, List<Action<T, object[]>>>(expectedEventTypes);
    }

    public EventManager()
        : this(5, 5)
    {
    }

    // NW: We cannot use single instances of arrays of each length because Event triggers might trigger inside other event triggers.
    // Using a single array would cause it to get overwritten, corrupting it.
    // We also use 3 different array pools (rather than just passing in m_reusableParams3Pool with 2 null arguments)
    // because some use-cases validate the length of the params.
    private readonly PoolFactory<object[]> m_reusableParams1Pool = new PoolFactory<object[]>(() => new object[1], x => { }, 5);
    private readonly PoolFactory<object[]> m_reusableParams2Pool = new PoolFactory<object[]>(() => new object[2], x => { }, 1);
    private readonly PoolFactory<object[]> m_reusableParams3Pool = new PoolFactory<object[]>(() => new object[3], x => { }, 1);
    private readonly object[] m_reusableParam0 = new object[0];

    public void RegisterEvent(T eventType, Action<T, object[]> listener)
    {
        List<Action<T, object[]>> list;
        if (!m_events.TryGetValue(eventType, out list))
        {
            list = m_listenerPool.Unpool();
            list.Add(listener);
            m_events.Add(eventType, list);
        }
        else
        {
            if (!list.Contains(listener))
            {
                list.Add(listener);
            }
            else
            {
                Debug.LogWarning("EventManager (RegisterEvent) :: Duplicate event listener: " + eventType + ", " + listener.Target + "." + listener.Method);
            }
        }
    }

    // ~120.
    public void TriggerEvent(T eventType)
    {
        TriggerEvent(eventType, m_reusableParam0);
    }

    // ~115.
    public void TriggerEvent(T eventType, object param1)
    {
        var arr = m_reusableParams1Pool.Unpool();
        arr[0] = param1;

        TriggerEvent(eventType, arr);

        arr[0] = null;
        m_reusableParams1Pool.Pool(arr);
    }

    // ~30.
    public void TriggerEvent(T eventType, object param1, object param2)
    {
        var arr = m_reusableParams2Pool.Unpool();
        arr[0] = param1;
        arr[1] = param2;

        TriggerEvent(eventType, arr);

        arr[0] = null;
        arr[1] = null;
        m_reusableParams2Pool.Pool(arr);
    }

    // ~10.
    public void TriggerEvent(T eventType, object param1, object param2, object param3)
    {
        var arr = m_reusableParams3Pool.Unpool();
        arr[0] = param1;
        arr[1] = param2;
        arr[2] = param3;

        TriggerEvent(eventType, arr);

        arr[0] = null;
        arr[1] = null;
        arr[2] = null;
        m_reusableParams3Pool.Pool(arr);
    }

    // ~5.
    public void TriggerEvent(T eventType, params object[] optParams)
    {
        List<Action<T, object[]>> list;
        m_events.TryGetValue(eventType, out list);

        // Debug.Log(optParams.Aggregate("TriggerEvent(<color=cyan>" + eventType + ":: ", (c, n) => c + "\"" + n + "\", ") + "</color>) " + (list != null ? list.Count.ToString() : "null"));

        if (list != null)
        {
            if (list.Count == 0)
            {
                return;
            }

            // Make a copy of this list, as it may get modified by the action.
            var copy = m_listenerPool.Unpool();
            copy.AddRange(list);

            foreach (var action in copy)
            {
                action(eventType, optParams);
            }

            copy.Clear();
            m_listenerPool.Pool(copy);
        }
    }

    public void DeregisterEvent(T eventType, Action<T, object[]> listener)
    {
        List<Action<T, object[]>> list;
        if (m_events.TryGetValue(eventType, out list))
        {
            list.Remove(listener);

            // NW: Really, we SHOULD log when we fail to remove, but it's spammy. 
            // Debug.LogWarning("EventManager (DeregisterEvent) :: (Harmless) Cannot remove listener as it's not in the list: " + eventType + ", " + listener.Target + "." + listener.Method);

            // Unregister the event itself and re-pool the list.
            // This should speed up event lookups and reduce memory.
            if (list.Count == 0)
            {
                m_events.Remove(eventType);
                m_listenerPool.Pool(list); // List is empty when we pool it.
            }
        }
    }

    public Dictionary<T, List<Action<T, object[]>>> GetRegisteredEvents()
    {
        return m_events;
    }
}
