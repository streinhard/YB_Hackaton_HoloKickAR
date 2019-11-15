using UnityEngine;
using System.Collections;
using System;

public class CoroutineTransition : IGenericTransition
{
    protected GenericStateMachine _callbackStateMachine = null;
    protected Func<Action, IEnumerator> _transitionCoroutineFunction = null;
    protected MonoBehaviour _coroutineExecuter = null;
    protected Coroutine _runningCoroutine = null;

    public CoroutineTransition(Func<Action, IEnumerator> transitionCoroutineFunction, MonoBehaviour coroutineExecuter)
    {
        Debug.Assert(transitionCoroutineFunction != null, "CoroutineTransition.Constructor: transitionCoroutineFunction is null");
        Debug.Assert(coroutineExecuter != null, "CoroutineTransition.Constructor: coroutineExecuter is null");

        _transitionCoroutineFunction = transitionCoroutineFunction;
        _coroutineExecuter = coroutineExecuter;
    }
    protected void CoroutineEndedCallback()
    {
        _runningCoroutine = null;
        _callbackStateMachine.OnTransitionEnded();
    }

    #region IGenericTransition
    public virtual void InitTransition(GenericStateMachine callbackStateMachine, int fromId, int toId)
    {
        Debug.Assert(callbackStateMachine != null, "TweenTransition.InitTransition: callbackStateMachine is null");
        _callbackStateMachine = callbackStateMachine;
    }

    public virtual bool DoesSupportAbort()
    {
        return true;
    }
    public virtual void StartTransition()
    {
        Debug.Assert(_runningCoroutine == null, "CoroutineTransition.StartTransition: _runningCoroutine is not null");

        // start coroutine (end is handled in callback
        _runningCoroutine = _coroutineExecuter.StartCoroutine(_transitionCoroutineFunction(this.CoroutineEndedCallback));
    }
    public virtual bool IsInTransition()
    {
        return _runningCoroutine != null;
    }
    public virtual bool AbortTransition()
    {
        if(_runningCoroutine != null)
        {
            _coroutineExecuter.StopCoroutine(_runningCoroutine);
            _runningCoroutine = null;
            _callbackStateMachine.OnTransitionAborted();
            return true;
        }
        return false;
    }

    public virtual bool DoesSupportForceFinish()
    {
        return true;
    }

    public virtual bool ForceFinishTransition()
    {
        if (_runningCoroutine != null)
        {
            _coroutineExecuter.StopCoroutine(_runningCoroutine);
            _runningCoroutine = null;
            _callbackStateMachine.OnTransitionEnded();
            return true;
        }
        return false;
    }
    #endregion
}
