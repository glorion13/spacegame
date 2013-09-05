using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : BuildableEntity {

    public List<Vector2> MovementTrajectory;

    public override void OnDragStart()
    {
        MovementTrajectory.Clear();
    }
    public override void OnDrag(Vector2 dragCoordinates)
    {
            Debug.Log(dragCoordinates);
            MovementTrajectory.Add(new Vector2(dragCoordinates.x, dragCoordinates.y));
    }
    public override void OnDragFinish()
    {
        Move();
    }

    public void Move()
    {
        foreach (var point in MovementTrajectory)
        {
            SetNewPosition((int) point.x, (int) point.y);
        }
    }
	
	// Update is called once per frame
    public override void Update()
    {
        base.Update();
	}
}
