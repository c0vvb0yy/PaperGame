using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayGroupStats : MonoBehaviour
{
    private TextMeshProUGUI groupStats;

    public BattleUnit PartyMember;

    private void Start() 
    {
        groupStats = GetComponentInChildren<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }

    private void OnEnable() 
    {
        if(groupStats)
            groupStats.text = $"{PartyMember.UnitName}: HP {PartyMember.CurrentHP}/{PartyMember.MaxHP}\nAtk: {PartyMember.AtkDamage} Def:{PartyMember.Defense}";
    }

}
