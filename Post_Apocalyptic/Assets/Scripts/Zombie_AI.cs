using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Zombie_Movement))]
public class Zombie_AI : MonoBehaviour
{
    private Zombie_Movement movement;
    private Zombie_AV zombie_av;

    //All zombie actions
    public enum Zombie_State
    {
        Wander,
        Attacking,
        Chasing
    }
    [SerializeField] private float think_every_seconds = 0.2f;
    //Indicates zombies current action
    public Zombie_State zombie_state = Zombie_State.Wander;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Zombie_Movement>();
        zombie_av = GetComponent<Zombie_AV>();

        StartCoroutine(Think());
    }

    //Decide what to do
    IEnumerator Think()
    {
        while(true)
        {
            switch (zombie_state)
            {
                case Zombie_State.Wander:
                    //Change State
                    if(movement.IsTargetInRange())
                    {
                        zombie_state = Zombie_State.Chasing;
                    }
                    //State Commands
                    movement.MoveAround();
                    zombie_av.MoveAnimation(false);
                    zombie_av.AttackAnimation(false);
                    break;
                case Zombie_State.Chasing:
                    //Change State
                    if (!movement.IsTargetInRange())
                    {
                        zombie_state = Zombie_State.Wander;
                    }
                    if(movement.HasReachedDestination())
                    {
                        zombie_state = Zombie_State.Attacking;
                    }
                    //State Commands
                    movement.ChaseTarget();
                    zombie_av.MoveAnimation(true);
                    zombie_av.AttackAnimation(false);
                    break;
                case Zombie_State.Attacking:
                    if (!movement.HasReachedDestination())
                    {
                        Debug.Log(movement.HasReachedDestination());
                        zombie_state = Zombie_State.Wander;
                    }
                    zombie_av.AttackAnimation(true);
                    break;
            }
            yield return new WaitForSeconds(think_every_seconds);
        }
    }
}
