using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float JumpForce = 2f;
    public float Speed = 5f;

    public Rigidbody Body;

    public LayerMask WhatIsGround;
    public Transform GroundPoint;
    public bool IsGrounded;

    private Vector2 MoveInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() 
    {
        MoveInput.x = Input.GetAxisRaw("Horizontal");
        MoveInput.y = Input.GetAxisRaw("Vertical");
        MoveInput.Normalize();

        Body.velocity = new Vector3(MoveInput.x, Body.velocity.y, MoveInput.y) * Speed;
        
        RaycastHit hit;
        if(Physics.Raycast(GroundPoint.position, Vector3.down, out hit, 2f, WhatIsGround))
        {
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Body.velocity += new Vector3(0f, JumpForce, 0f);
        }
    }
}
