using UnityEngine;
using System;

public class ActionTransition: IGenericTransition
{
    protected Action _transitionAction = null;
    protected GenericStateMachine _callbackStateMachine = null;
    protected bool _transitionStarted = false;


    public ActionTransition(Action transitionAction)
    {
        _transitionAction = transitionAction;
    }

    #region IGenericTransition
    public virtual void InitTransition(GenericStateMachine callbackStateMachine, int fromId, int toId)
    {
        Debug.Assert(callbackStateMachine != null, "TweenTransition.InitTransition: callbackStateMachine is null");
        _callbackStateMachine = callbackStateMachine;
    }

    public virtual bool DoesSupportAbort()
    {
        return false;
    }
    public virtual void StartTransition()
    {
        _transitionStarted = true;
        _transitionAction();
        if(_callbackStateMachine != null)
        {
            _transitionStarted = false;
            _callbackStateMachine.OnTransitionEnded();
        }
        else
        {
            _transitionStarted = false;
        }
    }
    public virtual bool IsInTransition()
    {
        return _transitionStarted;
    }
    public virtual bool AbortTransition()
    {
        // does not support abort
        return false;
    }

    public virtual bool DoesSupportForceFinish()
    {
        return false;
    }

    public virtual bool ForceFinishTransition()
    {
        return false;
    }
    #endregion
}
