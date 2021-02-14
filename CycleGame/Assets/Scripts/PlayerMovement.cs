using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

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

    public Animation FrontAnim;
    public Animation FrontWalk;
    public Animation FrontJump;
    public Animation FrontFall;
    public Animation BackAnim;
    public Animation BackWalk;
    public Animation BackJump;
    public Animation BackFall;
    public SpriteAnimation Animator;

    private Flip flip;

    // Start is called before the first frame update
    void Start()
    {
        flip = GetComponent<Flip>();
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
            if(WalkedBack)
            {
                Animator.Animation = BackAnim;
                Animator.AnimationReset();
                FacingFront = false;
            }
            else
            {
                Animator.Animation = FrontAnim;
                Animator.AnimationReset();
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
            if(WalkedBack)
            {
                Animator.AnimationReset();
                Animator.Animation = BackAnim;
            }
            else
            {
                Animator.AnimationReset();
                Animator.Animation = FrontAnim;
            }
        }
        else //when theres ANY KIND of movement happening
        {
            if(FacingFront)
            {
                if(IsJumping)
                {
                    Animator.AnimationReset();
                    Animator.Animation = FrontJump;
                }
                else if(IsFalling)
                {
                    Animator.AnimationReset();
                    Animator.Animation = FrontFall;
                }
                else
                {
                    if(Animator.Animation != FrontWalk)
                    {
                        Animator.AnimationReset();
                    }
                    Animator.Animation = FrontWalk;
                }
            }
            else
            {
                if(IsJumping)
                {
                    Animator.AnimationReset();
                    Animator.Animation = BackJump;
                }
                else if(IsFalling)
                {
                    Animator.AnimationReset();
                    Animator.Animation = BackFall;
                }
                else
                {
                    if(Animator.Animation != BackWalk)
                    {
                        Animator.AnimationReset();
                    }
                    Animator.Animation = BackWalk;
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
        Vector3 finalPos = Step.transform.localPosition;
        Vector3 velocity = Body.velocity;
        var stepPos = Step.transform.localPosition;

        finalPos += velocity*1.5f;

        finalPos = new Vector3(Mathf.Clamp(finalPos.x, -6f, 6f), 0f,0f);
        stepPos = new Vector3(Mathf.Clamp(stepPos.x, -6f, 6f), 0f,0f);
        Step.transform.localPosition = Vector3.SmoothDamp(stepPos, finalPos, ref velocity, 2f);

    }
}