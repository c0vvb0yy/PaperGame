using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{

    public Rigidbody Target;
    public NavMeshAgent Body;

    public bool FacingRight = true;
    public bool FacingFront = true;
    public bool WalkedBack = false;

    public Animation FrontAnim;
    public Animation FrontWalk;
    public Animation BackAnim;
    public Animation BackWalk;
    public SpriteAnimation Animator;

    private Flip flip;

    // Start is called before the first frame update
    void Start()
    {
        flip = GetComponent<Flip>();
    }

    void Update()
    {
        Body.SetDestination(Target.position);

        DetermineAnimation();

        flip?.FlipSprite(FacingRight, FacingFront);
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
    }
    
    public void DetermineAnimation()
    {
        DetermineDirection();
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
        else
        {
            if(FacingFront)
            {
                Animator.Animation = FrontWalk;
            }
            else
            {
                Animator.Animation = BackWalk;
            }
        }
    }
}
