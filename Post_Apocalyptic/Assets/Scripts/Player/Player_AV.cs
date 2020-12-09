using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*This scripts handles all the required animations and sound effects */

[RequireComponent(typeof(Animator))]

public class Player_AV : MonoBehaviour
{
    //Scope_Canvas for show/hide
    [SerializeField] GameObject scope_canvas;

    //For storing camera values
    int old_culling;
    float old_fov;

    //Components
    private Animator anim;

    // Initialize
    void Start()
    {
        anim = GetComponent<Animator>();

        //Default camera values
        old_culling = Camera.main.cullingMask;
        old_fov = Camera.main.fieldOfView;
    }

    public void IdleAnimation()
    {
        
    }

    //Play Movement Animation
    public void MoveAnimation(float _speed)
    {
        anim.SetFloat("move_speed", Mathf.Abs(_speed));
    }

    //Play Shooting Animation
    public void ShootAnimation()
    {
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Scoping"))
        {
            anim.Play("Shoot");
        }
        
    }

    //Scope Animation
    public void ScopeAnimation()
    {
        anim.SetBool("is_scope", true);
    }

    //Show Scope Canvas
    public void ShowScopeCanvas()
    {
        //Show Canvas
        scope_canvas.SetActive(true);
        //Change field of view (scope)
        Camera.main.fieldOfView = 40f;
        //Change cullingMask (what camera sees) to not see player gun
        Camera.main.cullingMask = (1 << LayerMask.NameToLayer("TransparentFX")) |
                                  (1 << LayerMask.NameToLayer("Default")) |
                                  (1 << LayerMask.NameToLayer("Ignore Raycast")) |
                                  (1 << LayerMask.NameToLayer("Water")) |
                                  (1 << LayerMask.NameToLayer("UI")) |
                                  (1 << LayerMask.NameToLayer("Ground"));
    }

    //Hide Canvas and reset camera values
    public void HideScopeCanvas()
    {
        anim.SetBool("is_scope", false);
        Camera.main.fieldOfView = old_fov;
        Camera.main.cullingMask = old_culling;
        scope_canvas.SetActive(false);
    }
}
