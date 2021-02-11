using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn.Unity;

public class SpeakerPortrait : MonoBehaviour
{
    public GameObject LeftPortrait;
    public GameObject RightPortrait;
    public RectTransform SpeakerContainer;

    public float Speed;

    private Image LeftSprite;
    private Image RightSprite;

    private RectTransform LeftPos;
    private RectTransform RightPos;

    private Vector3 LeftEndPos;
    private Vector3 RightEndPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GetComponents()
    {
        LeftSprite = LeftPortrait.GetComponent<Image>();
        RightSprite = RightPortrait.GetComponent<Image>();
        LeftPos = LeftPortrait.GetComponent<RectTransform>();
        RightPos = RightPortrait.GetComponent<RectTransform>();
        LeftEndPos = LeftPos.localPosition;
        RightEndPos = RightPos.localPosition;
    }

    public void SetCorrectPosition()
    {
        LeftPos.localPosition = LeftEndPos;
        RightPos.localPosition = RightEndPos;
    }
    
    private void Update() 
    {
        if(LeftEndPos != LeftPos.localPosition)
        {
            var pos = LeftPos.localPosition;
            pos = Vector3.Lerp(pos, LeftEndPos, Speed);
            LeftPos.localPosition = pos;
        }
        if(RightEndPos != RightPos.localPosition)
        {
            var pos = RightPos.localPosition;
            pos = Vector3.Lerp(pos, RightEndPos, Speed);
            RightPos.localPosition = pos;
        }
    }

    [YarnCommand("SetLeftSpeaker")]
    public void UpdateSpeakerPortraitL(string speaker)
    {
        var portrait = Resources.Load<Sprite>("Talk_Sprites/"+speaker+"_talk");
        LeftSprite.sprite = portrait;
        var nullPos = LeftPos.localPosition;
        nullPos.x = -1350f;
        LeftPos.localPosition = nullPos; 
    }

    [YarnCommand("SetRightSpeaker")]
    public void UpdateSpeakerPortraitR(string speaker)
    {
        var portrait = Resources.Load<Sprite>("Talk_Sprites/"+speaker+"_talk");
        RightSprite.sprite = portrait;
        var nullPosr = RightPos.localPosition;
        nullPosr.x = 1350f;
        RightPos.localPosition = nullPosr;
    }

    [YarnCommand("Darken")]
    public void DarkenPortrait(string side)
    {
        Color darkened = new Color(.25f, .25f, .25f);
        Color normal = new Color(1, 1, 1);
        var pos = SpeakerContainer.position;
        switch (side)
        {
            case "left":
                LeftSprite.color = darkened;
                RightSprite.color = normal;
                pos.x = 1700;
                SpeakerContainer.position = pos;
                Debug.Log("Darnkekned");
            break;
            default:
                LeftSprite.color = normal;
                RightSprite.color = darkened;
                pos.x = 216;
                SpeakerContainer.position = pos;
                Debug.Log("darkend");
            break;
        }
    }
}
