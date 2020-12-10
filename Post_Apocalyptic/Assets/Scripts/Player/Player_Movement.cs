using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This script handles the player movement. It contains all the essential functions for movement.
  It requires a CharacterController and a Player_Input script in order to work */

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Player_Input))]

public class Player_Movement : MonoBehaviour
{
    [Header("Movement Requirements:")]
    [SerializeField] private Transform ground_check;
    [SerializeField] private float ground_distance;
    [SerializeField] private LayerMask ground_mask;

    [Header("Movement Configuration:")]
    [SerializeField] private float gravity = -35f;
    [SerializeField] private float move_speed = 10f;
    [SerializeField] private float run_speed = 17f;
    [SerializeField] private float jump_height = 2f;

    //Needed Components
    private CharacterController controller;

    //Private Variables
    private float current_speed = 0f;
    private Vector3 velocity = Vector3.zero;
    private bool is_grounded;


    //Initialization:
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        current_speed = move_speed;
    }

    //This method applies gravity to player
    public void GravityApplication()
    {
        //Checks if player is touching ground.
        is_grounded = Physics.CheckSphere(ground_check.position, ground_distance, ground_mask);
        //If player is on ground apply a little force to him.
        if (is_grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //velocity adds the gravity acceleration each second
        velocity.y += gravity * Time.deltaTime;
        //Apply Gravity to player : Based to the Free Fall Equation
        controller.Move(velocity * Time.deltaTime);
    }

    //Moves the player on the desired direction
    public void MovePlayer(Vector3 _dir)
    {
        Vector3 move = _dir * current_speed;
        controller.Move(move * current_speed * Time.deltaTime);
    }

    //Jump based on the vertical Jump Physics Equation
    public void Jump()
    {
        if(is_grounded)
        {
            velocity.y = Mathf.Sqrt(jump_height * -2f * gravity);
        }
        
    }

    //Change from Sprint to Walk
    public void Sprint(bool is_runing)
    {
        if(is_runing)
        {
            current_speed = run_speed;
        }
        else
        {
            current_speed = move_speed;
        }
    }
}
