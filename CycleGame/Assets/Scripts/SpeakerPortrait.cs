using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeakerPortrait : MonoBehaviour
{
    public Image Portrait;
    public TextMeshProUGUI Name;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateSpeakerPortrait()
    {
        var speaker = Name.text;
        if(speaker == "Zaavan")
        {
            var portrait = (Sprite) Resources.Load<Sprite>("Talk_Sprites/Zaavan_talk");
            Debug.Log(portrait.name);
            Portrait.sprite = portrait;
        }
        if(speaker == "Anthrazit")
        {
            var portrait = Resources.Load<Sprite>("Talk_Sprites/Anthrazit_talk");
            Portrait.sprite = portrait;
        }
    }
}
