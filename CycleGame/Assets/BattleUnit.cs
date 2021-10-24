using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public string UnitName;
    
    public int BaseAtkDamage;
    public int ModDamage;
    public int AtkDamage;

    public int BaseDefense;
    public int ModDefense;
    public int Defense;

    public int MaxHP;
    public int CurrentHP;

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

    public void AddAttack(int value)
    {
        ModDamage += value;
        CalculateAtkDamage();
    }
    public void SubtractAttack(int value)
    {
        ModDamage -= value;
        CalculateAtkDamage();
    }
    public void AddDefense(int value)
    {
        ModDefense += value;
        CalculateDefense();
    }
    public void SubtractDefense(int value)
    {
        ModDefense -= value;
        CalculateDefense();
    }

    public void CalculateAtkDamage()
    {
        AtkDamage = BaseAtkDamage + ModDamage;
    }
    public void CalculateDefense()
    {
        Defense = BaseDefense + ModDefense;
    }
}
