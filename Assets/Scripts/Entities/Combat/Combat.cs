using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combat : Entity
{
    public List<List<GameObject>> playersInCombat;

    public Combat(GameObject unit1, GameObject unit2)
    {
        playersInCombat.Add(new List<GameObject>());
        playersInCombat.Add(new List<GameObject>());
        playersInCombat[0].Add(unit1);
        playersInCombat[1].Add(unit2);
    }

}