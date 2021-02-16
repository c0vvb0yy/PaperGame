using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleStatsUI : MonoBehaviour
{
    public TextMeshProUGUI NameText;

    public void SetInfo(BattleUnit unit)
    {
        var info = $"{unit.UnitName}: {unit.CurrentHP}/{unit.MaxHP} <3";
        NameText.text = info;
    }

}
