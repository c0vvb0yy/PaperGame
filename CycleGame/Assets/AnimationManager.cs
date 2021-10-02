using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementAnimations
{
    FRONTWALK, BACKWALK, FRONTJUMP, BACKJUMP, FRONTFALL, BACKFALL
}

public class AnimationManager : MonoBehaviour
{
    public Animation[] MovementAnims;
    public Animation FrontFacingAnimtion;
    public Animation BackFacingAnimation;
    //public Animation[] IdleAnimations;
    private SpriteAnimation Animator;
    private PlayerMovement PlayerMovement;
    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponentInChildren<SpriteAnimation>();
        PlayerMovement = GetComponent<PlayerMovement>();
    }

    public void SetAnimation(Animation animation)
    {
        if(Animator.Animation == animation)
            return;
        Animator.AnimationReset();
        Animator.Animation = animation;
    }
    
    public void PlayMovementAnimation(MovementAnimations walkAnim)
    {
        if(Animator.Animation == MovementAnims[(int)walkAnim])
            return;
        Animator.AnimationReset();
        Animator.Animation = MovementAnims[(int)walkAnim];
    }

    public void PlayDefaultAnimation()
    {
        if(PlayerMovement.WalkedBack)
        {
            SetAnimation(BackFacingAnimation);
        }
        else
        {
            SetAnimation(FrontFacingAnimtion);
        }
    }
}