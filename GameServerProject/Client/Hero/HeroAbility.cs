using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAbility
{
    int _id;
    string _title = "";
    string _description = "";
    HeroAbilityPassive _hAPassive;

    public HeroAbility(string title, string desc)
    {
        Debug.Log("ability made");
        _title = title;
        _description = desc;
        _hAPassive = new HeroAbilityPassive("null", "null");
    }
    public HeroAbilityPassive GetPassive()
    {
        return _hAPassive;
    }
    public void SetPassive(HeroAbilityPassive passive)
    {
        Debug.Log("passive set");
        _hAPassive = passive;
    }

    public void SetDesc(string desc)
    {
        _description = desc;
    }

    public void SetTitle(string title)
    {
        _title = title;
    }

    public string GetTitle()
    {
        return _title;
    }

    public string GetDesc()
    {
        return _description;
    }

    public void SetID(int id)
    {
        _id = id;
    }

    public int GetID()
    {
        return _id;
    }
}
