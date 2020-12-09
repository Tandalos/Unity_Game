using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This script allows the player to look around*/

public class Player_Look : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private Transform player_obj;
    [Header("Configuration")]
    [SerializeField] private float sensitivity = 180f;
    [SerializeField] private float min_vert_clamp = -85f;
    [SerializeField] private float max_vert_clamp = 85f;

    //Private Variables
    private float horizontal_rot = 0f;

    // Initialization
    void Start()
    {
        //Locks the cursor to game and it hides it.
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        LookAround();
    }

    private void LookAround()
    {
        //Get Mouse Input (Both Horizontal and Vertical)
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        //Calculate camera rotation on x Axis
        horizontal_rot -= mouseY;
        //Clamp rotation
        horizontal_rot = Mathf.Clamp(horizontal_rot, min_vert_clamp, max_vert_clamp);
        //Apply rotation to camera
        transform.localRotation = Quaternion.Euler(horizontal_rot, 0f, 0f);
        //Rotate the whole player object on y Axis
        player_obj.Rotate(Vector3.up * mouseX);
    }
}
