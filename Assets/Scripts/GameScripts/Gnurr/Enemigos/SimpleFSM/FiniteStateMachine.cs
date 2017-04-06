using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine<T> : State<T> where T: MonoBehaviour
{


    private Dictionary<string, State<T>> _states;
    private List<string> _events;

    private string _currentState;

    public FiniteStateMachine(T executor) : base(executor)
    {
        _states = new Dictionary<string, State<T>>();
        _events = new List<string>();
    }

    public override void Init()
    {
        _states[_currentState].Init();
    }

    public override void Update()
    {
        _states[_currentState].Update();
        ProcessEvents();
    }

    public override void End()
    {

    }

    public string CurrentState
    {
        get { return _currentState; }
    }

    public void AddState(State<T> state, bool init = false)
    {
        if (_states.Count == 0 || init)
            _currentState = state.GetType().Name;

        _states.Add(state.GetType().Name, state);
        state.AddExecutor(this);

    }

    public bool AddTransition(string o, string des, string a_event)
    {
        State<T> state;
        bool result = false;
        if(_states.TryGetValue(o,out state))
        {
            if (_states.ContainsKey(des))
            {
                state.AddTransition(a_event, des);
                result = true;
            }
        }
        return result;
    }

    public override void Emmit(string a_event)
    {
        _events.Add(a_event);
    }

    protected void ProcessEvents()
    {
        for(int i = 0; i < _events.Count; ++i)
        {
            string e = _events[i];
            string newState = _states[_currentState].Transite(e);
            if (newState != null)
            {
                _states[_currentState].End();
                _currentState = newState;
                _states[_currentState].Init();
            }
        }
        _events.Clear();
    }
}
