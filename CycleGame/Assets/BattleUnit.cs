using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public string UnitName;
    
    public int AtkDamage;
    public int Defense;

    public int MaxHP;
    public int CurrentHP;

    private void Start() {
        
    }

    public bool TakeDamage(int damage)
    {
        CurrentHP -= damage;
        if(CurrentHP <= 0)
            return true;

        return false;
    }

    public void Heal(int amount)
    {
        if(CurrentHP + amount >= MaxHP)
            CurrentHP = MaxHP;
        else
            CurrentHP += amount;
    }
}
