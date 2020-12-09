using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_AV : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void MoveAnimation(bool is_chasing)
    {
        anim.applyRootMotion = false;
        anim.SetBool("is_chasing", is_chasing);
    }

    public void AttackAnimation(bool is_attacking)
    {
        anim.applyRootMotion = true;
        anim.SetBool("is_attacking", is_attacking);
    }
}
