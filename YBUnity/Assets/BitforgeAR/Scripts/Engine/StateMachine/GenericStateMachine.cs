using System;
using System.Collections.Generic;
using UnityEngine;

public class GenericStateMachine
{
    private Dictionary<int, GenericState> _states = null;
    private GenericState _currentState = null;
    private GenericState _nextState = null;
    private IGenericTransition _currentTransition = null;
    private Action<GenericState> _onInit = null;
    private Action<GenericState, GenericState> _onStateChangeStartedAction = null;
    private Action<GenericState, GenericState> _onStateChangeEndedAction = null;

    public GenericState CurrentState
    {
        get { return _currentState; }
    }
    public GenericState NextState
    {
        get
        {
            if (_currentTransition != null && _nextState != null)
            {
                return _nextState;
            }

            return null;
        }
    }

    public IGenericTransition CurrentTransition
    {
        get { return _currentTransition; }
    }

    public bool IsInTransition
    {
        get { return _currentTransition != null; }
    }

    private bool _isPrepared = false;

    public GenericStateMachine(Action<GenericState> onInit = null, Action < GenericState, GenericState> onStateChangeStarted = null, Action <GenericState, GenericState> onStateChangeEnded = null)
    {
        _onInit = onInit;
        _onStateChangeStartedAction = onStateChangeStarted;
        _onStateChangeEndedAction = onStateChangeEnded;
    }

    public void Prepare()
    {
        foreach (var pair in _states)
        {
            if(pair.Value != null)
            {
                pair.Value.Prepare(this);
            }
        }
        _isPrepared = true;
    }

    public GenericState AddState(int stateId, string name)
    {
        GenericState result = null;

        if (_states == null || !_states.TryGetValue(stateId, out result))
        {
            return AddState(new GenericState(stateId, name));
        }

        return result;
    }

    public GenericState AddState(GenericState state)
    {
        //Debug.Assert(state != null, "BStateMachine.AddState: state is null", this);
        if (state != null)
        {
            // create state set if nessesary
            if (_states == null)
            {
                _states = new Dictionary<int, GenericState>();
            }

            // add state to set
            if (!_states.ContainsKey(state.Id))
            {
                _states.Add(state.Id, state);
                _isPrepared = false;
            }
        }

        return state;
    }

    public bool DeleteState(GenericState state)
    {
        if (state != null)
        {
            return DeleteState(state.Id);
        }
        return false;
    }

    public bool DeleteState(int stateId)
    {
        if (_states != null)
        {
            bool removed = _states.Remove(stateId);
            _isPrepared &= removed;
            return removed;
        }
        return false;
    }

    public bool RequestStateChange(int targetId)
    {
        GenericState requestedState = null;
        if (_states != null && _states.TryGetValue(targetId, out requestedState))
        {
            // state machine is in transition so handle it
            if (IsInTransition)
            {
                // target is current state (start state in transition)
                if (targetId == _currentState.Id)
                {
                    // abort transition if possible
                    if (_currentTransition.DoesSupportAbort())
                    {
                        if(_currentTransition.AbortTransition())
                        {
                            return true;
                        }
                        return false;
                    }
                    else
                    {
                        // transition could not be aborted, so request is rejected
                        return false;
                    }
                }
                // target is same as actual transition target, so just reject request
                else if (_nextState != null && _nextState.Id == targetId)
                {
                    // same same, reject
                    return false;
                }

                // in transition, but nothing to do
                return false;
            }

            // is it intialization
            if (_currentState == null)
            {
                // fake a state change ended without any transition
                return StartTransition(requestedState);
            }
            else
            {
                // no queuing needed, transition can be processed directly
                if (requestedState != _currentState)
                {
                    if (_currentState.CanTransitionTo(requestedState))
                    {
                        return StartTransition(requestedState);
                    }
                    else
                    {
                        // conditions not fullfilled, request is rejected
                        //Debug.LogWarningFormat("GenericStateMachine.RequestStateChange({0}): is rejected currentState is {1} <color='yellow'>NO TRANSITION</color>", requestedState, _currentState);
                        return false;
                    }
                }
                else
                {
                    // conditions not fullfilled, request is rejected
                    return false;
                }
            }
        }

        // conditions not fullfilled, request is rejected
        Debug.LogWarningFormat("GenericStateMachine.RequestStateChange({0}):<color='yellow'>target does not exit in state machine</color>", requestedState, _currentState);
        return false;
    }

    public bool IsInStateOrInTransitionTo(GenericState state)
    {
        if (state != null)
        {
            return IsInStateOrInTransitionTo(state.Id);
        }
        return false;
    }

    public bool IsInStateOrInTransitionTo(int stateId)
    {
        if (IsInTransitionTo(stateId) || (!IsInTransition && _currentState != null && _currentState.Id == stateId))
        {
            return true;
        }
        return false;
    }

    public bool IsInTransitionTo(GenericState state)
    {
        if (state != null)
        {
            return IsInTransitionTo(state.Id);
        }
        return false;
    }

    public bool IsInTransitionTo(int stateId)
    {
        if (IsInTransition && NextState != null && NextState.Id == stateId)
        {
            return true;
        }
        return false;
    }

    protected bool StartTransition(GenericState targetState)
    {
        // check preparation of state machine
        if (!_isPrepared)
        {
            Prepare();
        }

        if (_currentState == null)
        {
            // it is the initial transition, means initialization of the state machine
            // there is no transition to be started we directly push the end state
            _nextState = targetState;
            OnInit();
            return true;
        }
        else
        {
            Debug.AssertFormat(_currentTransition == null, "GenericStateMachine.StartTransition: _currentTransition{0} is not null!", _nextState);

            // check if a transition is possible
            if (_currentState != null && _currentState.GetTransitionTo(targetState.Id, out _currentTransition))
            {
                _nextState = targetState;

                if (_onStateChangeStartedAction != null)
                {
                    _onStateChangeStartedAction(_currentState, _nextState);
                }

                if (_currentState.OnExitAction != null)
                {
                    _currentState.OnExitAction(_nextState);
                }

                _currentTransition.StartTransition();
                return true;
            }
        }

        return false;
    }

    protected void OnInit()
    {
        _currentState = _nextState;

        if (_onInit != null )
        {
            _onInit(_currentState);
        }
    }

    public void OnTransitionEnded()
    {
        // save old state for state change event
        var oldState = _currentState;

        _currentState = _nextState;
        _nextState = null;
        _currentTransition = null;

        if (_currentState.OnEnterAction != null)
        {
            _currentState.OnEnterAction(oldState);
        }

        if (_onStateChangeEndedAction != null)
        {
            _onStateChangeEndedAction(oldState, _currentState);
        }
    }

    public void OnTransitionAborted()
    {
        // keep actual state
        _currentTransition = null;

        // still call an enter action, because exit was called as well
        if (_currentState.OnEnterAction != null)
        {
            _currentState.OnEnterAction(_currentState);
        }

        if (_onStateChangeEndedAction != null)
        {
            _onStateChangeEndedAction(_nextState, _currentState);
        }

        _nextState = null;
    }
}