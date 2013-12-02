using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : BuildableEntity
{
    public List<Vector2> MovementTrajectory;

    protected bool DoMove;

    public int MovementSpeed;
    public int Power;
    public int Armour;
    public int HP;


    #region Controls logic
    public override void OnDragStart()
    {
        MovementTrajectory.Clear();
    }

    public override void OnDrag(Vector2 dragCoordinates)
    {
        if (MovementTrajectory.Count == 0)
            MovementTrajectory.Add(new Vector2(dragCoordinates.x, dragCoordinates.y));
        else
            AddInBetweenPointsToMovementTrajectory(MovementTrajectory[MovementTrajectory.Count - 1].x, MovementTrajectory[MovementTrajectory.Count - 1].y, dragCoordinates.x, dragCoordinates.y);
    }

    public override void OnDragFinish()
    {
        DoMove = true;
    }
    #endregion

    private void Move()
    {
        for (int i = 0; i < MovementSpeed; i++)
        {
            if (MovementTrajectory.Count > i)
            {
                SetNewPosition((int)MovementTrajectory[i].x, (int)MovementTrajectory[i].y);
                MovementTrajectory.RemoveAt(i);
            }
            else
                DoMove = false;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (DoMove)
            Move();
    }

    private void AddInBetweenPointsToMovementTrajectory(float x0, float y0, float x1, float y1)
    {
        int dy = (int)(y1 - y0);
        int dx = (int)(x1 - x0);
        int stepx, stepy;

        if (dy < 0) { dy = -dy; stepy = -1; }
        else { stepy = 1; }
        if (dx < 0) { dx = -dx; stepx = -1; }
        else { stepx = 1; }
        dy <<= 1;
        dx <<= 1;

        float fraction = 0;

        MovementTrajectory.Add(new Vector2(x0, y0));
        if (dx > dy)
        {
            fraction = dy - (dx >> 1);
            while (Mathf.Abs(x0 - x1) > 1)
            {
                if (fraction >= 0)
                {
                    y0 += stepy;
                    fraction -= dx;
                }
                x0 += stepx;
                fraction += dy;
                MovementTrajectory.Add(new Vector2(x0, y0));
            }
        }
        else
        {
            fraction = dx - (dy >> 1);
            while (Mathf.Abs(y0 - y1) > 1)
            {
                if (fraction >= 0)
                {
                    x0 += stepx;
                    fraction -= dy;
                }
                y0 += stepy;
                fraction += dx;
                MovementTrajectory.Add(new Vector2(x0, y0)); ;
            }
        }
    }
}
