using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMStateSystem : MonoBehaviour
{
    private GMState _currentState;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetState(GMState state)
    {
        _currentState = state;
        StartCoroutine(_currentState.Start());
    }
    // Update is called once per frame
    void Update()
    {
        StartCoroutine(_currentState.Update());
    }
}
