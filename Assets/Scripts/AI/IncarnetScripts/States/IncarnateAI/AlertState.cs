using System.Collections;
using UnityEngine;

// Pauses the agent when they spot the target
public class AlertState : OverworldState
{
    public AlertState(OverworldStateSystem system) : base(system)
    {
    }
    public override IEnumerator Start()
    {
        //Play alert animation
        _system.transform.LookAt(_system.fov.GetTargetFromTag("Player"));
        _system.nav.SetDestination(_system.transform.position);
        yield return new WaitForSeconds(0.4f);
        //Perform alert functions
        if (_system.fov.GetTargetFromTag("Player") == null)
        {
            _system.SetState(new WanderState(_system));
        }
        else
        {
            _system.SetState(new PursuitState(_system));
        }
        
        yield break;
    }
}