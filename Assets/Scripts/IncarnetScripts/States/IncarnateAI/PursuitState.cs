using System.Collections;
using UnityEngine;

//namespace Assets.Scripts.IncarnetScripts.States
//{
public class PursuitState : OverworldState
{
    public Transform target;
    public PursuitState(OverworldStateSystem system) : base(system)
    {
    }
    public override IEnumerator Start()
    {
        target = _system.fov.GetTargetFromTag("Player");
        _system.target = target;
        if (target == null)
        {
            Debug.LogError("Something really dungooffed");
        }
        _system.fov.viewRadius = _system.targetedSightDistance;
        yield break;
    }
    public override IEnumerator Move()
    {
        //do xyz
        target = _system.fov.GetTargetFromTag("Player");
        if (target != null)
        {
            _system.nav.SetDestination(target.position);
        }
        else
        {
            _system.target = null;
            _system.SetState(new WanderState(_system));
        }
        
        yield break;
    }
}
//}
