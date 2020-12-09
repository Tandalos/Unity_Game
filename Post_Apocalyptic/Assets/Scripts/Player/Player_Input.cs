using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This script is handling the player input and apply the corresponding functions from Player_Movement.
  It requires a Player_Movement script to work */

[RequireComponent(typeof(Player_Movement))]
[RequireComponent(typeof(Player_Shoot))]
[RequireComponent(typeof(Player_AV))]

public class Player_Input : MonoBehaviour
{
    [Header("Player Keys")]
    [SerializeField] KeyCode jump_button = KeyCode.Space;
    [SerializeField] KeyCode sprint_button = KeyCode.LeftShift;
    [SerializeField] KeyCode shoot_button = KeyCode.Mouse0;
    [SerializeField] KeyCode scope_button = KeyCode.Mouse1;

    [Header("Multi-axis speed normalization")]
    [SerializeField] float movement_normalization = 0.05f;

    //Needed Components
    private Player_Movement movement;
    private Player_AV p_av;
    private Player_Shoot shoot;

    // Initialization
    void Start()
    {
        movement = GetComponent<Player_Movement>();
        p_av = GetComponent<Player_AV>();
        shoot = GetComponent<Player_Shoot>();
        
    }

    // Update is called once per frame
    void Update()
    {
        movement.GravityApplication();
        MovementInput();
        SprintInput();
        JumpInput();
        ScopeInput();
        ShootInput();
    }

    private void MovementInput()
    {
        //Get axis
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Speed normalization
        if(Mathf.Abs(Mathf.Abs(x) - Mathf.Abs(z)) <= movement_normalization)
        {
            x = x / 2;
            z = z / 2;
        }

        //Apply movement
        movement.MovePlayer(transform.right * x + transform.forward * z);

        //Apply animations
        p_av.MoveAnimation((x + z) / 2f);
    }

    private void JumpInput()
    {
        if(Input.GetKeyDown(jump_button))
        {
            movement.Jump();
        }
    }

    private void SprintInput()
    {
        if(Input.GetKey(sprint_button))
        {
            movement.Sprint(true);
        }
        else if(Input.GetKeyUp(sprint_button))
        {
            movement.Sprint(false);
        }
    }

    private void ScopeInput()
    {
        if(Input.GetKey(scope_button))
        {
            p_av.ScopeAnimation();
        }
        else
        {
            p_av.HideScopeCanvas();
        }
    }

    private void ShootInput()
    {
        if(Input.GetKeyDown(shoot_button))
        {
            p_av.ShootAnimation();
            shoot.Shoot();
        }
    }

}
