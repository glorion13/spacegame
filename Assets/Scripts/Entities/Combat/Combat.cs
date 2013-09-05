using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combat : Entity
{
    public List<GameObject> side1Units;
    public List<GameObject> side2Units;

    public Combat(GameObject unit1, GameObject unit2)
    {
        side1Units.Add(unit1);
        side2Units.Add(unit2);
    }

}