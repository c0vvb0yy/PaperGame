using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[RequireComponent (typeof(AnimationManager))]
[RequireComponent(typeof(Flip))]
public class PlayerMovement : MonoBehaviour
{
    public float Speed = 5f;
    public float JumpForce = 5f;
    public float FallMultiplier = 2.5f;
    public float LowJumpMultiplier = 2f;
    private float FallSpeed;

    public Rigidbody Body;
    public Transform Visuals;
    public Transform Step;

    public LayerMask WhatIsGround;
    public Transform GroundPoint;
    public bool IsGrounded;
    public bool IsFalling = false;
    public bool IsJumping = false;

    public bool FacingRight = true;
    public bool FacingFront = true;
    public bool WalkedBack;
    private bool JumpPressed;

    public Vector2 MoveInput;
    public bool IsWalking;

    //public MovementAnimations MovementAnimations;









    private Flip flip;
    private AnimationManager animationManager;

    // Start is called before the first frame update
    void Start()
    {
        flip = GetComponent<Flip>();
        animationManager = GetComponent<AnimationManager>();

    }

    // Update is called once per frame
    void Update()
    {
        JumpPressed |= Input.GetKeyDown(KeyCode.Space);
        flip.FlipSprite(FacingRight, FacingFront);
    }

    private void FixedUpdate() 
    {
        if(PlayerManager.FreeToMove)
        {
            MoveInput.x = Input.GetAxisRaw("Horizontal");
            MoveInput.y = Input.GetAxisRaw("Vertical");
            MoveInput.Normalize();
            

            Body.velocity = new Vector3(MoveInput.x, 0f, MoveInput.y) * Speed;

            //Check For Ground
            RaycastHit hit;
            if(Physics.Raycast(GroundPoint.position, Vector3.down, out hit, .5f, WhatIsGround))
            {
                IsGrounded = true;
                IsFalling = false;
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

            DetermineAnimations();
            NextPos();

            JumpPressed = false;

        }
        else
        {
            Body.velocity = Vector3.zero;
            animationManager.PlayDefaultAnimation();
            if(WalkedBack)
            {


                FacingFront = false;
            }
            else
            {


                FacingFront = true;
            }
        }
    }

    public void Flip()
    {
        Vector3 rotation = Visuals.eulerAngles;
        rotation.y = Mathf.MoveTowards(rotation.y, FacingRight ? 0 : 180, 360 * Time.deltaTime); 
        Visuals.eulerAngles = rotation;
    }

    [YarnCommand("AwkwardLeave")]
    public void AwkwardLeave()
    {
        Flip();
    }

    public void DetermineAnimations()
    {
        DetermineDirection();
        //If NOT MOVING AT ALL
        if(Body.velocity == Vector3.zero)
        {
            animationManager.PlayDefaultAnimation();
            return;
        }
        else //when theres ANY KIND of movement happening
        {
            if(FacingFront)
            {
                if(IsJumping)
                {
                    animationManager.PlayMovementAnimation(MovementAnimations.FRONTJUMP);
                }
                else if(IsFalling)
                {
                    animationManager.PlayMovementAnimation(MovementAnimations.FRONTFALL);
                }
                else
                {
                    //animationManager.SetAnimation(FrontWalk);
                    animationManager.PlayMovementAnimation(MovementAnimations.FRONTWALK);
                }
            }
            else
            {
                if(IsJumping)
                {
                    animationManager.PlayMovementAnimation(MovementAnimations.BACKJUMP);
                }
                else if(IsFalling)
                {
                    animationManager.PlayMovementAnimation(MovementAnimations.BACKFALL);
                }
                else
                {
                    animationManager.PlayMovementAnimation(MovementAnimations.BACKWALK);

                }
            }
        }
    }

    public void DetermineDirection()
    {
        if(Body.velocity.x < 0 && FacingRight)
        {
            FacingRight = false;
        }
        if(Body.velocity.x > 0 && !FacingRight)
        {
            FacingRight = true;
        }
        if(Body.velocity.z > 0 && FacingFront)
        {
            FacingFront = false;
            WalkedBack = true;
        }
        if(Body.velocity.z < 0 && !FacingFront)
        {
            FacingFront = true;
            WalkedBack = false;
        }
        if(IsGrounded)
        {
            IsFalling = false;
            IsJumping = false;
        }
        if(Body.velocity.y < 0 && !IsGrounded)
        {
            IsFalling = true;
            IsJumping = false;
        }
        if(Body.velocity.y > 0 && IsGrounded)
        {
            IsJumping = true;
        }
        
    }

    public void NextPos()
    {
        var localPosition = Step.transform.localPosition;
        Vector3 finalPos = localPosition;
        Vector3 velocity = Body.velocity;
        var stepPos = localPosition;

        finalPos += velocity*1.5f;

        finalPos = new Vector3(Mathf.Clamp(finalPos.x, -6f, 6f), 0f,0f);
        stepPos = new Vector3(Mathf.Clamp(stepPos.x, -6f, 6f), 0f,0f);
        localPosition = Vector3.SmoothDamp(stepPos, finalPos, ref velocity, 2f);
        Step.transform.localPosition = localPosition;
    }
}