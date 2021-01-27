using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 5f;
    public float JumpForce = 5f;
    public float FallMultiplier = 2.5f;
    public float LowJumpMultiplier = 2f;
    private float FallSpeed;

    public Rigidbody Body;
    public Transform Visuals;

    public LayerMask WhatIsGround;
    public Transform GroundPoint;
    public bool IsGrounded;

    public bool FacingRight = true;
    private bool JumpPressed;

    public Vector2 MoveInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        JumpPressed |= Input.GetKeyDown(KeyCode.Space);
        Flip();
    }

    private void FixedUpdate() 
    {
        MoveInput.x = Input.GetAxisRaw("Horizontal");
        MoveInput.y = Input.GetAxisRaw("Vertical");
        MoveInput.Normalize();

        Body.velocity = new Vector3(MoveInput.x, 0f, MoveInput.y) * Speed;
        
        //Direction Check/Flip
        if(Body.velocity.x < 0 && FacingRight)
        {
            FacingRight = false;
        }
        if(Body.velocity.x > 0 && !FacingRight)
        {
            FacingRight = true;
        }

        //Check For Ground
        RaycastHit hit;
        if(Physics.Raycast(GroundPoint.position, Vector3.down, out hit, .5f, WhatIsGround))
        {
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }
        
        //All things Jumping
        if(JumpPressed && IsGrounded)
        {
            FallSpeed = JumpForce;
        }
        if(!IsGrounded || FallSpeed > 0)
        {
            float gravityMultiplier = 1f;
            if(FallSpeed < 0)
            {
                gravityMultiplier = FallMultiplier;
            }
            else if(Body.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                gravityMultiplier = LowJumpMultiplier;
            }
            FallSpeed += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
            Body.velocity += new Vector3(0f, FallSpeed, 0f);
        }
        else
        {
            FallSpeed = 0f;
        }
        JumpPressed = false;
    }

    public void Flip()
    {
        Vector3 rotation = Visuals.eulerAngles;
        rotation.y = Mathf.MoveTowards(rotation.y, FacingRight ? 0 : 180, 360 * Time.deltaTime); 
        Visuals.eulerAngles = rotation;
    }
}
