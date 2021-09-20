using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayGroupStats : MonoBehaviour
{
    private TextMeshProUGUI groupStats;

    private BattleUnit zaav;
    private BattleUnit anthra;
    

    private void Start() 
    {
        groupStats = GetComponentInChildren<TextMeshProUGUI>();    
        zaav = GameObject.FindWithTag("Player").GetComponent<BattleUnit>();
        anthra = GameObject.FindWithTag("Anthra").GetComponent<BattleUnit>();  
        gameObject.SetActive(false);
    }

    private void OnEnable() 
    {
        groupStats.text = $"{zaav.UnitName}: HP {zaav.CurrentHP}/{zaav.MaxHP}\n\n{anthra.UnitName}: HP {anthra.CurrentHP}/{anthra.MaxHP}";
    }

}
