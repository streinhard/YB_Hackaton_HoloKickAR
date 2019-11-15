using System;
using System.Collections.Generic;

public class GenericState
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public Action<GenericState> OnEnterAction { get; set; }
    public Action<GenericState> OnExitAction { get; set; }

    private Dictionary<int, IGenericTransition> _transitions = null;

    public GenericState(int id, string name, Action<GenericState> onEnterAction =  null, Action<GenericState> onExitAction = null)
    {
        Id = id;
        Name = name;
        OnEnterAction = onEnterAction;
        OnExitAction = onExitAction;
    }

    public void Prepare(GenericStateMachine genericStateMachine)
    {
        if (_transitions != null)
        {
            foreach (var transitionPair in _transitions)
            {
                if (transitionPair.Value != null)
                {
                    transitionPair.Value.InitTransition(genericStateMachine, Id, transitionPair.Key);
                }
            }
        }
    }

    public bool AddTransition(GenericState targetState, IGenericTransition transition)
    {
        if(targetState != null)
        {
            return AddTransition(targetState.Id, transition);
        }
        return false;
    }
    public bool AddTransition(int targetId, IGenericTransition transition)
    {
        // create target set if nessesary
        if (_transitions == null)
        {
            _transitions = new Dictionary<int, IGenericTransition>();
        }

        // append new target
        if (!_transitions.ContainsKey(targetId))
        {
            _transitions.Add(targetId, transition);
            return true;
        }

        return false;
    }

    public bool DeleteTransition(GenericState targetState)
    {
        if (targetState != null)
        {
            return DeleteTransition(targetState.Id);
        }
        return false;
    }
    public bool DeleteTransition(int targetId)
    {
        if (_transitions != null)
        {
            return _transitions.Remove(targetId);
        }
        return false;
    }

    public bool ReplaceTransition(GenericState targetState, IGenericTransition transition)
    {
        if (targetState != null)
        {
            return ReplaceTransition(targetState.Id, transition);
        }
        return false;
    }
    public bool ReplaceTransition(int targetId, IGenericTransition transition)
    {
        if (_transitions != null)
        {
            _transitions.Remove(targetId);
            _transitions.Add(targetId, transition);
            return true;
        }
        return false;
    }

    public bool GetTransitionTo(GenericState targetState, out IGenericTransition transition)
    {
        if(targetState != null)
        {
            return GetTransitionTo(targetState.Id, out transition);
        }

        transition = null;
        return false;
    }
    public bool GetTransitionTo(int targetId, out IGenericTransition transition)
    {
        if (_transitions != null && _transitions.TryGetValue(targetId, out transition))
        {
            return true;
        }

        transition = null;
        return false;
    }
    
    public bool CanTransitionTo(GenericState targetState)
    {
        if (targetState != null)
        {
            return CanTransitionTo(targetState.Id);
        }
        return false;
    }
    public bool CanTransitionTo(int targetId)
    {
        return _transitions != null && _transitions.ContainsKey(targetId);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        if(obj is GenericState)
        {
            return ((GenericState)obj).Id == Id;
        }

        return false;
    }

    public override string ToString()
    {
        return Name.ToString();
    }
}