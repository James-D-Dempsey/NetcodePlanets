using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : NetworkBehaviour
{
    public CharacterController controller;

    public Transform groundCheck;
    public float groundDistance = 0.04f;
    public LayerMask groundMask;
    bool isGrounded;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    Vector3 velocity;

    private int currentSceneIndex;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }


        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (currentSceneIndex != SceneManager.GetActiveScene().buildIndex)
        {
            Debug.Log("worked");
            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SetGravity(currentSceneIndex);
            SetGravityClientRpc(currentSceneIndex);
        }
    }

    private void SetGravity(int newGravity)
    {
        Debug.Log("Changing Gravity");
        switch (newGravity)
        {
            case 0:
                gravity = -9.81f;
                jumpHeight = 3f;
                break;
            case 1:
                gravity = -1.62f;
                jumpHeight = 16.6875f;
                break;
            case 2:
                gravity = -23.1f;
                jumpHeight = 1.1558f;
                break;
            default:
                gravity = -9.81f; 
                break;
        }
        
    }

    [ClientRpc]
    public void SetGravityClientRpc(int newGravity)
    {
        SetGravity(newGravity);
        
    }

}
