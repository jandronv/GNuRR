using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMExecutor<T> : MonoBehaviour where T: MonoBehaviour{

    public T _component;
    private FiniteStateMachine<T> _fsm;
	// Use this for initialization
	public virtual void Start () {
        _fsm = new FiniteStateMachine<T>(_component);
        CreateStates(_fsm);
        _fsm.Init();
    }

    // Update is called once per frame
    public void Update() {
        _fsm.Update();
        _Update(_fsm);
    }

    protected virtual void _Update(FiniteStateMachine<T> fsm)
    {

    }

    protected abstract void CreateStates(FiniteStateMachine<T> fsm);

    void OnDestroy()
    {
        _fsm.End();
    }
}
