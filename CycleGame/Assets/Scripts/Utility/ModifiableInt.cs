using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ModifiedEvent();
[System.Serializable]
public class ModifiableInt
{
    [SerializeField]
    private int baseValue;
    public int BaseValue {get { return baseValue; } set { baseValue = value; UpdateModifiedValue(); } }

    [SerializeField]
    private int modifiedValue;
    public int ModifiedValue{get { return modifiedValue; } private set { modifiedValue = value; /*UpdateModifiedValue();*/ } }

    public List<IModifiers> Modifiers = new List<IModifiers>();

    public event ModifiedEvent ValueModified;
    public ModifiableInt(ModifiedEvent method = null)
    {
        modifiedValue = BaseValue;
        if(method != null)
        {
            ValueModified += method;
        }
    }

    public void RegisterModEvent(ModifiedEvent method)
    {
        ValueModified += method;
    }

    public void UnregisterModEvent(ModifiedEvent method)
    {
        ValueModified -= method;
    }

    public void UpdateModifiedValue()
    {
        var valueToAdd = 0;
        for (int i = 0; i < Modifiers.Count; i++)
        {
            Modifiers[i].AddValue(ref valueToAdd);
        }
        ModifiedValue = baseValue + valueToAdd;
        if(ValueModified != null)
            ValueModified.Invoke();
    }

    public void AddModifier(IModifiers _modifier)
    {
        Modifiers.Add(_modifier);
        UpdateModifiedValue();
    }
    public void RemoveModifier(IModifiers _modifier)
    {
        Modifiers.Remove(_modifier);
        UpdateModifiedValue();
    }
}
