using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSystem : MonoBehaviour
{
    private OverworldState _currentState;
    public void SetState(OverworldState state)
    {
        _currentState = state;
        StartCoroutine(_currentState.Start());
    }
}
