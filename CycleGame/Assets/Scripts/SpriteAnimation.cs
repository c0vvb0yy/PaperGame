using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(MeshSprite))]
[ExecuteAlways]
public class SpriteAnimation : MonoBehaviour
{
    private MeshSprite MeshSprite;

    public Animation Animation;

    private int CurrentFrame;
    private float FrameProgress;
    public bool Dirty;

    // Start is called before the first frame update
    void Start()
    {
        MeshSprite = GetComponent<MeshSprite>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        FrameProgress += Time.deltaTime;
        int loopCount = 0;
        while (FrameProgress > Animation.KeyFrames[CurrentFrame].Duration || Dirty)
        {
            FrameProgress -= Animation.KeyFrames[CurrentFrame].Duration;
            CurrentFrame = (CurrentFrame + 1) % Animation.KeyFrames.Length; 
            MeshSprite.Sprite = Animation.KeyFrames[CurrentFrame].Sprite;
            MeshSprite.UpdateMPB();
            Dirty = false;
            loopCount++;
            if(loopCount > 3)  //safety shit number can be played around with
                break;
        }
    }

    public void AnimationReset() 
    {
        CurrentFrame = 0;
        FrameProgress = 0;
        Dirty = true;
    }

    private void OnValidate() 
    {
        Dirty = true;
    }
}
