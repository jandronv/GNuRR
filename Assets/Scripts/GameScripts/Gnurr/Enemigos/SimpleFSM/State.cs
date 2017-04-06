using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State<T> where T : MonoBehaviour
{
    private T _component;
    private FiniteStateMachine<T> _executor;

    private Dictionary<string, string> _transition;

    public State(T component)
    {
        _component = component;
        _transition = new Dictionary<string, string>();
    }

    public void AddExecutor(FiniteStateMachine<T> ex)
    {
        _executor = ex;
    }

    public virtual void Emmit(string a_event)
    {
        _executor.Emmit(a_event);
    }

    public string Transite(string a_event)
    {
        if (_transition.ContainsKey(a_event))
            return _transition[a_event];

        return null;
    }

    public string this[string s]
    {
        get { return _transition[s]; }
    }

    public string GetState(string s)
    {
        return _transition[s];
    }

    public void AddTransition(string a_event, string state)
    {
        if (_transition.ContainsKey(a_event))
            _transition[a_event] = state;
        else
            _transition.Add(a_event,state);
    }

    public T Component
    {
        get { return _component; }
    }

    public virtual void Init()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void End()
    {

    }
}
