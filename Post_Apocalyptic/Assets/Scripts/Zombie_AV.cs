using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script handles all sounds and animations of zombies */
public class Zombie_AV : MonoBehaviour
{
    //Required Components
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    //Aply movement(false) and chasing(true) animations
    public void MoveAnimation(bool is_chasing)
    {
        //Unlock object's movement, so it can move while an animation is playing
        anim.applyRootMotion = false;
        anim.SetBool("is_chasing", is_chasing);
    }

    //Aply attack animation
    public void AttackAnimation(bool is_attacking)
    {
        //Lock object's movement, so it cant move while an animation is playing
        anim.applyRootMotion = true;
        anim.SetBool("is_attacking", is_attacking);
    }
}
