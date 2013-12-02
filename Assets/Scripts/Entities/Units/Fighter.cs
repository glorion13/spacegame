using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fighter : Unit
{
    // Use this for initialization
    public override void Start()
    {
        base.Start();

        // Set up LoS template
        // left tile, right tile, top tile, bottom tile
        /*LineOfSightTemplate = new List<Tuple<int, int>>
        {
            new Tuple<int, int>(0, 0),
            new Tuple<int, int>(-1, 0),
            new Tuple<int, int>(0, -1),
            new Tuple<int, int>(1, 0),
            new Tuple<int, int>(0, 1)
        };*/
        LineOfSightTemplate = new List<Tuple<int, int>>
        {
            new Tuple<int, int>(0, 0),
            new Tuple<int, int>(-1, 0),
            new Tuple<int, int>(0, -1),
            new Tuple<int, int>(1, 0),
            new Tuple<int, int>(0, 1),
            new Tuple<int, int>(-2, 0),
            new Tuple<int, int>(0, -2),
            new Tuple<int, int>(2, 0),
            new Tuple<int, int>(0, 2),
            new Tuple<int, int>(-1, -1),
            new Tuple<int, int>(-1, -2),
            new Tuple<int, int>(-2, -1),
            new Tuple<int, int>(1, 1),
            new Tuple<int, int>(1, 2),
            new Tuple<int, int>(2, 1),
            new Tuple<int, int>(1, -1),
            new Tuple<int, int>(1, -2),
            new Tuple<int, int>(2, -1),
            new Tuple<int, int>(-1, 1),
            new Tuple<int, int>(-1, 2),
            new Tuple<int, int>(-2, 1),
        };
        InitialiseLineOfSight(X, Y);
    }

}
