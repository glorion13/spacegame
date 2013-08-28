using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : BuildableEntity {

	// Use this for initialization
    public override void Start()
    {
        base.Start();

        // Set up LoS template
        // left tile, right tile, top tile, bottom tile
        LineOfSightTemplate = new List<Tuple<int, int>>
        {
            new Tuple<int, int>(0, 0),
            new Tuple<int, int>(-1, 0),
            new Tuple<int, int>(0, -1),
            new Tuple<int, int>(1, 0),
            new Tuple<int, int>(0, 1)
        };
	}
	
	// Update is called once per frame
    public override void Update()
    {
        base.Update();
	}
}
