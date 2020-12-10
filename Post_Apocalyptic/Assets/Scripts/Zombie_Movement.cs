using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(Zombie_AI))]
[RequireComponent(typeof(NavMeshAgent))]
public class Zombie_Movement : MonoBehaviour
{
    [Header("Chasing Configuration:")]
    [SerializeField] private float chase_range;
    [SerializeField] private float chase_acceleration;
    [Header("Wander Configuration:")]
    [SerializeField] private float wander_range;
    [SerializeField] private float wander_time;
    [SerializeField] private float wander_acceleration;
    [Header("Zombie's Target:")]
    [SerializeField] private Transform target;
    
    //Required Component
    NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    //Checks If target is in range
    public bool IsTargetInRange()
    {
        //If target is inside AI's range
        if (Vector3.Distance(transform.position, target.position) < chase_range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Starts the Wander Coroutine
    public void MoveAround()
    {
        agent.acceleration = wander_acceleration;
        StartCoroutine(Wander());
    }

    //Chase Target If it's in range
    public void ChaseTarget()
    {
        if(IsTargetInRange())
        {
            agent.acceleration = chase_acceleration;
            //Start Following Player
            agent.SetDestination(target.position);
        }
    }

    //Start Wandering (Move to random positions inside NavMesh area)
    IEnumerator Wander()
    {
        //Set destination to a random spot
        Vector3 new_pos = RandomNavSphere(transform.position, wander_range, -1);
        agent.SetDestination(new_pos);
        //Change position every wander_time seconds
        yield return new WaitForSeconds(wander_time);
    }

    //Sets AI's destination in a random spot inside the walkable NavMesh area
    private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        //Create an invisible Check Sphere of specified range
        Vector3 randDirection = Random.insideUnitSphere * dist;
        //Direction changes based on current position
        randDirection += origin;
        //Get a random spot inside the check sphere
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        //Return the destination
        return navHit.position;
    }

    //Checks if AI has reached its destination
    public bool HasReachedDestination()
    {
        //If you arived on the desired position
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            //Set again the AI's destination (Because when it reaches its destination it resets)
            agent.destination = target.position;
            return true;
        }
        else
        {
            return false;
        }
    }
}
