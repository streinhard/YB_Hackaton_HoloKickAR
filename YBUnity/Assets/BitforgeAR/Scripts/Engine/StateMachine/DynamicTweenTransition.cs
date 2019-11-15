using System;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// Tween is create every time the transition is used and killed afterwards
/// </summary>
public class DynamicTweenTransition : StaticTweenTransition
{
    protected int _fromId;
    protected int _toId;
    protected Func<int, int, Tween> _generateTweenFunc = null;

    public DynamicTweenTransition(Func<int, int, Tween> generateTweenFunc, string name = null, float reverseTimeScale = 0.5f) : base(null, name, reverseTimeScale) {
        Debug.Assert(generateTweenFunc != null, "DynamicTweenTransition.Constructor: generateTweenFunc is null");

        _generateTweenFunc = generateTweenFunc;
    }

    protected override void TweenEndedForwardCallback()
    {
        base.TweenEndedForwardCallback();
        _tween = null;
    }
    protected override void TweenEndedBackwardsCallback()
    {
        base.TweenEndedBackwardsCallback();
        _tween.Kill();
        _tween = null;
    }

    #region IGenericTransition
    public override void InitTransition(GenericStateMachine callbackStateMachine, int fromId, int toId)
    {
        Debug.Assert(callbackStateMachine != null, "DynamicTweenTransition.InitTRansition: callbackStateMachine is null");
        _callbackStateMachine = callbackStateMachine;

        _fromId = fromId;
        _toId = toId;
    }

    public override void StartTransition()
    {
        // kill tween if still running
        if(_tween != null)
        {
            _tween.Kill();
            _tween = null;
        }

        // get tween from function
        _tween = _generateTweenFunc(_fromId, _toId);
        _tween.SetAutoKill(true);
        Debug.AssertFormat(_tween != null, "TweenTransition.StartTransition: generateTweenFunc returns null for tween {0} to {1}", _fromId, _toId);

        // append callbacks
        _tween.OnComplete(TweenEndedForwardCallback);
        _tween.OnRewind(TweenEndedBackwardsCallback);

        // play tween
        _tween.Play();
    }

    public override bool IsInTransition()
    {
        if(_tween != null)
        {
            return base.IsInTransition();
        }

        return false;
    }

    public override bool AbortTransition()
    {
        if (_tween != null)
        {
            return base.AbortTransition();
        }

        return false;
    }

    public override bool ForceFinishTransition()
    {
        if (_tween != null)
        {
            return base.ForceFinishTransition();
        }

        return false;
    }

    #endregion IGenericTransition
}
