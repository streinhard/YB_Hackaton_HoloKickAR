using DG.Tweening;
using UnityEngine;


/// <summary>
/// Tween is assigned once and never changed again
/// </summary>
public class StaticTweenTransition : IGenericTransition
{
    protected GenericStateMachine _callbackStateMachine = null;
    protected Tween _tween = null;
    protected float _reverseTimeScale;
    protected string _name = null;

    public StaticTweenTransition(Tween tween, string name = null, float reverseTimeScale = 0.5f)
    {
        _tween = tween;
        _name = name;
        _reverseTimeScale = reverseTimeScale;
    }

    protected virtual void TweenEndedForwardCallback()
    {
        if (!_tween.isBackwards)
        {
            _callbackStateMachine.OnTransitionEnded();
        }
    }
    protected virtual void TweenEndedBackwardsCallback()
    {
        if (_tween.isBackwards)
        {
            _callbackStateMachine.OnTransitionAborted();
        }
    }

    #region IGenericTransition
    public virtual void InitTransition(GenericStateMachine callbackStateMachine, int fromId, int toId)
    {
        Debug.Assert(callbackStateMachine != null, "StaticTweenTransition.InitTRansition: callbackStateMachine is null");
        _callbackStateMachine = callbackStateMachine;

        Debug.Assert(_tween != null, "StaticTweenTransition.Constructor: tween is null");
        _tween.OnComplete(TweenEndedForwardCallback);
        _tween.OnRewind(TweenEndedBackwardsCallback);
    }

    public virtual bool DoesSupportAbort()
    {
        return true;
    }

    public virtual void StartTransition()
    {
        Debug.Assert(!_tween.IsPlaying(), "StaticTweenTransition.StartTransition: _tween still running");

        // init tween
        if (_tween.isBackwards)
        {
            _tween.Flip();
        }
        _tween.Rewind();
        _tween.timeScale = 1f;

        // play tween
        _tween.Play();
    }

    public virtual bool IsInTransition()
    {
        return _tween.IsPlaying();
    }

    public virtual bool AbortTransition()
    {
        if (_tween.IsPlaying() && _tween.fullPosition > 0)
        {
            // switch tween to backwards
            _tween.Pause();
            _tween.timeScale = _reverseTimeScale;
            _tween.Flip();
            _tween.Play();
        }
        else
        {
            _tween.Pause();
            _callbackStateMachine.OnTransitionAborted();
        }
        return true;
    }

    public virtual bool DoesSupportForceFinish()
    {
        return true;
    }

    public virtual bool ForceFinishTransition()
    {
        if (_tween.IsPlaying() && _tween.fullPosition > 0)
        {
            _tween.Complete(); //will also call the transition ended callback
        }
        else
        {
            _tween.Pause();
            _callbackStateMachine.OnTransitionEnded();
        }
        return true;
    }

    #endregion IGenericTransition
}