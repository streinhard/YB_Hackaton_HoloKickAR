using UnityEngine;

public class EmptyTransition : IGenericTransition
{
    protected GenericStateMachine _callbackStateMachine = null;

    #region IGenericTransition
    public void InitTransition(GenericStateMachine callbackStateMachine, int fromId, int toId)
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
        _callbackStateMachine.OnTransitionEnded();
    }
    public bool IsInTransition()
    {
        return false;
    }
    public virtual bool AbortTransition()
    {
        // abort not supported
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

