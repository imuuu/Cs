using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCollection 
{
    public Hero[] _heros;

    public void InitHeros(Hero[] heros)
    {
        //print("Heros inited");
        _heros = heros;
    }

    public Hero GetHero(int id)
    {
        if (id >= _heros.Length) return null;

        return _heros[id];
    }
}
