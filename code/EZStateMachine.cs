using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EZState<TSubtype> where TSubtype : EZState<TSubtype>, new()
{
    public static T Create<T>(EZStateMachine<TSubtype> stateMachine) where T : TSubtype, new()
    {
        T state = new T();
        state.StateMachine = stateMachine;
        return state;
    }
    
    public EZStateMachine<TSubtype> StateMachine { get; private set; }
    
    public abstract void Enter();
    public abstract void Exit();
    public abstract bool CanSwitchToState<T>() where T : TSubtype;
}

public class EZStateMachine<TSubtype> where TSubtype : EZState<TSubtype>, new()
{
    public delegate void StateChangedPreEnterEvent(TSubtype newState);
    public event StateChangedPreEnterEvent StateChangedPreEnter;
    public delegate void StateChangedEvent(TSubtype newState);
    public event StateChangedEvent StateChanged;

    public string Name { get; set; }
    public bool StateChangeLogging { get; set; } = true;

    private TSubtype currState { get; set; }
    private TSubtype nextState;

    public EZStateMachine(string name = null)
    {
        if (name == null)
            Name = GetType().Name;
        else
            Name = name;
    }

    public TSubtype GetState()
    {
        return currState;
    }

    public T GetStateAs<T>() where T : TSubtype
    {
        return currState as T;
    }

    public void UpdateNextState()
    {
        if (nextState != null)
        {
            currState?.Exit();
            currState = nextState;
            nextState = null;
            if (StateChangeLogging)
                Debug.LogFormat("[EZStateMachine {0}] switched to {1}", Name, currState?.GetType());
            StateChangedPreEnter?.Invoke(currState);
            currState.Enter();
            StateChanged?.Invoke(currState);
        }
    }

    public bool SwitchStateNext<T>(bool force = false) where T : TSubtype, new()
    {
        T next = EZState<TSubtype>.Create<T>(this);

        if (currState == null || currState.CanSwitchToState<T>() == true || force)
        {
            nextState = next;
            return true;
        }
        
        Debug.LogFormat("[EZStateMachine {0}] state {1} blocked switch to {2}", Name, currState.GetType(), next.GetType());
        return false;
    }

    public bool SwitchStateImmediate<T>(bool force = false) where T : TSubtype, new()
    {
        bool switched = SwitchStateNext<T>(force);
        if (switched)
            UpdateNextState();
        return switched;
    }
}
