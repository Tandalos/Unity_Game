using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(Zombie_AI))]
[RequireComponent(typeof(NavMeshAgent))]
public class Zombie_Movement : MonoBehaviour
{
    [SerializeField] private float chase_range;
    [SerializeField] private float wander_range;
    [SerializeField] private float wander_time;
    [SerializeField] private float move_acceleration;
    [SerializeField] private float run_acceleration;

    [SerializeField] private Transform target;

    NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void LateUpdate()
    {
        if(IsTargetInRange())
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion qDir = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, qDir, 40f * Time.deltaTime);
        }
    }

    public bool IsTargetInRange()
    {
        if (Vector3.Distance(transform.position, target.position) < chase_range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void MoveAround()
    {
        if (!agent.updateRotation)
        {
            agent.acceleration = move_acceleration;
            agent.updateRotation = true;
        }
        StartCoroutine(Wander());
    }

    public void StayIdle()
    {
        return;
    }

    public void ChaseTarget()
    {
        if(IsTargetInRange())
        {
            
            if(agent.updateRotation)
            {
                agent.updateRotation = false;
                agent.acceleration = run_acceleration;
            }
            
            agent.SetDestination(target.position);
        }
        
    }

    IEnumerator Wander()
    {

        Vector3 newPos = RandomNavSphere(transform.position, wander_range, -1);
        agent.SetDestination(newPos);

        yield return new WaitForSeconds(wander_time);
    }

    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    public bool HasReachedDestination()
    {
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.destination = target.position;
            return true;
        }
        else
        {
            return false;
        }
        
    }
}
