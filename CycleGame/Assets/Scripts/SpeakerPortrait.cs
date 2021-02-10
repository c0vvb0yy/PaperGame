using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn.Unity;

public class SpeakerPortrait : MonoBehaviour
{
    public Image LeftPortrait;
    public Image RightPortrait;
    public RectTransform SpeakerContainer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [YarnCommand("SetLeftSpeaker")]
    public void UpdateSpeakerPortraitL(string speaker)
    {
        var portrait = Resources.Load<Sprite>("Talk_Sprites/"+speaker+"_talk");
        LeftPortrait.sprite = portrait;
        
    }

    [YarnCommand("SetRightSpeaker")]
    public void UpdateSpeakerPortraitR(string speaker)
    {
        var portrait = Resources.Load<Sprite>("Talk_Sprites/"+speaker+"_talk");
        RightPortrait.sprite = portrait;
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
                LeftPortrait.color = darkened;
                RightPortrait.color = normal;
                pos.x = 1700;
                SpeakerContainer.position = pos;
                Debug.Log("Darnkekned");
            break;
            default:
                LeftPortrait.color = normal;
                RightPortrait.color = darkened;
                pos.x = 216;
                SpeakerContainer.position = pos;
                Debug.Log("darkend");
            break;
        }
    }
}
