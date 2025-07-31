
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepScript : MonoBehaviour
{
    public GameObject footstep;       
    public LayerMask groundLayer;        
    public float groundCheckDistance = 0.1f; 

    // Start is called before the first frame update
    void Start()
    {
        footstep.SetActive(false);
    }

    void Update()
    {
        if (IsGrounded())
        {
            if (Input.GetKey("w"))
            {
                footsteps();
            }

            if (Input.GetKeyDown("s"))
            {
                footsteps();
            }

            if (Input.GetKeyDown("a"))
            {
                footsteps();
            }

            if (Input.GetKeyDown("d"))
            {
                footsteps();
            }

            if (Input.GetKeyUp("w"))
            {
                StopFootsteps();
            }

            if (Input.GetKeyUp("s"))
            {
                StopFootsteps();
            }

            if (Input.GetKeyUp("a"))
            {
                StopFootsteps();
            }

            if (Input.GetKeyUp("d"))
            {
                StopFootsteps();
            }
        }
        else
        {
            StopFootsteps();
        }
    }

    void footsteps()
    {
        footstep.SetActive(true);
    }

    void StopFootsteps()
    {
        footstep.SetActive(false);
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }
}