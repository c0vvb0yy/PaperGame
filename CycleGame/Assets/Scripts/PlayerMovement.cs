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

    public bool FacingRight = true;
    public bool FacingFront = true;
    public bool WalkedBack;
    private bool JumpPressed;

    public Vector2 MoveInput;
    public bool IsWalking;
    public Vector3 LastPos;
    public Vector3 DeltaPosition;

    public Animation FrontAnim;
    public Animation FrontWalk;
    public Animation FrontJump;
    public Animation BackAnim;
    public Animation BackWalk;
    public SpriteAnimation Animator;

    private Flip flip;

    // Start is called before the first frame update
    void Start()
    {
        LastPos = this.transform.position;
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

            DeltaPosition = this.transform.position - LastPos;
            LastPos = this.transform.position;
            
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

            
            DetermineAnimations();
            
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
        if(FacingRight && Step.position.x > 0 || !FacingRight && Step.position.x < 0)
        {
            
            var step = Step.position;
            step.x = -step.x;
            Step.position = step;
        }
        this.transform.position = Vector3.MoveTowards(this.transform.position, Step.position, 1f);
    }

    private void DetermineAnimations()
    {
        
        if((JumpPressed && IsGrounded) || !IsGrounded)
        {
            Animator.Animation = FrontJump;
            if(IsGrounded)
                Animator.AnimationReset();
            return;
        }
        if((MoveInput.x == 0 && MoveInput.y == 0))
        {
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
            return;
        }
        if(IsGrounded)
        {
            bool walkingHorizontal = false;
            bool walkingBack = false;
            if(MoveInput.x != 0)
            {
                walkingHorizontal = true;
            }
            if(MoveInput.y > 0)
            {
                walkingBack = true;
                WalkedBack = true;
            }
            if(MoveInput.y < 0)
            {
                walkingBack = false;
                WalkedBack = false;
            }
            if(walkingBack) //for y back
            {
                Animator.Animation = BackWalk;
                //Animator.AnimationReset();
                FacingFront = false;
            }
            else //for y forward
            {
                Animator.Animation = FrontWalk;
                //Animator.AnimationReset();
                FacingFront = true;
            }
            if(walkingHorizontal && WalkedBack) //just x input but previously has walked back
            {
                Animator.Animation = BackWalk;
                //Animator.AnimationReset();
                FacingFront = false;
            }
            else if(walkingHorizontal && !WalkedBack) //just x input but previously has walked forward
            {
                Animator.Animation = FrontWalk;
                //Animator.AnimationReset();
                FacingFront = true;
            }
        }
    }
}