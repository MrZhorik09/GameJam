using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class StateManager<Estate>:MonoBehaviour where Estate:Enum
{
    protected Dictionary<Estate,BaseState<Estate>> States=new Dictionary<Estate, BaseState<Estate>>();
    protected BaseState<Estate> CurrentState;

    protected bool IsTransitioningState=false;
    void Awake()
    {
        CurrentState.EnterState();
    }

    void Update(){
        Estate nextStateKey = CurrentState.GetNextState();

        if (!IsTransitioningState&& nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        }else { 
            TransitionToState(nextStateKey);
        }
    }

    public void TransitionToState(Estate stateKey)
    {
        IsTransitioningState = true;
        CurrentState.ExitState();
        CurrentState=States[stateKey];
        CurrentState.EnterState();
        IsTransitioningState=false;
    }
    void OnTriggerEnter(Collider other){
        CurrentState.OnTriggerEnter(other);
    }
    void OnTriggerStay(Collider other) { 
        CurrentState.OnTriggerStay(other);
    }

    void OnTriggerExit(Collider other){
        CurrentState.OnTriggerExit(other);
    }


}
