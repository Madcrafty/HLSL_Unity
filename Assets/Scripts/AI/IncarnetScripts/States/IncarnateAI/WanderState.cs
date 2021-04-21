using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// This state makes the agent move to a random location
class WanderState : OverworldState
{
    public WanderState(OverworldStateSystem system) : base(system)
    {
    }
    public override IEnumerator Start()
    {
        _system.fov.viewRadius = _system.sightDistance;
        yield break;
    }
    // Move the agent
    public override IEnumerator Move()
    {
        if (_system.fov.visibleTargets != null && _system.fov.GetTargetFromTag("Player") != null)
        {
            _system.SetState(new AlertState(_system));
        }
        if (!_system.nav.hasPath)
        {
            _system.nav.SetDestination(RandomNavSphere(_system.transform.position, 20, -1));
        }
        yield break;
    }
    // Generate a random point to walk to
    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }
}