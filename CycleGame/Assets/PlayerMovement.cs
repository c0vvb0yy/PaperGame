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
    public Mesh Mesh;

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
            Flip();
        }
        if(Body.velocity.x > 0 && !FacingRight)
        {
            FacingRight = true;
            Flip();
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
        Vector3[] vertices = Mesh.vertices;
        for(int i = 0; i< vertices.Length; i++)
        {
            Vector3 current = vertices[i];
            current.y *= -1;
            vertices[i] = current;
        }

        Mesh.vertices = vertices;
        FlipNormals();
    }

    public void FlipNormals()
    {
        int[] triangles = Mesh.triangles;
        for(int i = 0; i < triangles.Length / 3; i++)
        {
            int a = triangles[i * 3 + 0];
            int b = triangles[i * 3 + 1];
            int c = triangles[i * 3 + 2];
            triangles[i * 3 + 0] = c;
            triangles[i * 3 + 1] = b;
            triangles[i * 3 + 2] = a;
        }
        Mesh.triangles = triangles;
    }
}
