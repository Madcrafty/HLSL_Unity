using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OverworldStateSystem : MonoBehaviour
{
    public GameObject HitObject;
    public float HitDistance;
    public float sightDistance;
    public float targetedSightDistance;
    public float distance;
    public Transform target;

    private RaycastHit sphereDetection;
    private OverworldState _currentState;

    public NavMeshAgent nav;
    //private IncarnateData data;
    public FieldOfView fov;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        //data = GetComponent<IncarnateData>();
        fov = GetComponent<FieldOfView>();
        StartCoroutine(fov.FindTargetsWithDelay(0.2f));
        //sightDistance = data.sightDistance;
        //targetedSightDistance = data.targetedSightDistance;
        SetState(new WanderState(this));
    }
    public void SetState(OverworldState state)
    {
        _currentState = state;
        StartCoroutine(_currentState.Start());
    }
    // change sphere casts to box casts
    void Update()
    {
        if (nav.desiredVelocity != new Vector3(0, 0, 0))
        {
            transform.rotation = Quaternion.LookRotation(nav.desiredVelocity.normalized);
        }
        Physics.SphereCast(transform.position, transform.localScale.x / 2, nav.desiredVelocity.normalized, out sphereDetection, distance);
        if (sphereDetection.transform != null)
        {
            HitObject = sphereDetection.transform.gameObject;
            HitDistance = sphereDetection.distance;
        }
        else
        {
            HitObject = null;
        }
        StartCoroutine(_currentState.Move());
    }
    private void OnDrawGizmos()
    {
        if (HitObject == null)
        {
            Gizmos.color = Color.green;
            //Gizmos.DrawLine(transform.position, transform.position + transform.forward * sightDistance);
            Gizmos.DrawLine(transform.position, transform.position + nav.desiredVelocity.normalized * distance);
            Gizmos.DrawWireSphere(transform.position + nav.desiredVelocity.normalized * distance, transform.localScale.x / 2);
            //Gizmos.DrawWireSphere(transform.position + transform.forward * sightDistance, transform.localScale.x / 2);
        }
        else
        {
            if (HitObject.tag == "Player")
            {
                Gizmos.color = Color.yellow;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawLine(transform.position, transform.position + nav.desiredVelocity.normalized * HitDistance);
            //Gizmos.DrawLine(transform.position, transform.position + transform.forward * HitDistance);
            Gizmos.DrawWireSphere(transform.position + nav.desiredVelocity.normalized * HitDistance, transform.localScale.x / 2);
            //Gizmos.DrawWireSphere(transform.position + transform.forward * HitDistance, transform.localScale.x / 2);
        }
    }
}
