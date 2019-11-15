using UnityEngine;
using System.Collections;
using System;

public interface IGenericTransition
{
    void InitTransition(GenericStateMachine callbackStateMachine, int fromId, int toId);

    bool DoesSupportAbort();
    bool DoesSupportForceFinish();
    bool IsInTransition();

    void StartTransition();
    bool AbortTransition();
    bool ForceFinishTransition();
}
