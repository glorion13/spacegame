using UnityEngine;
using System;

public class BuildableEntity : Entity {

    public int BuildProgress;
    public float BuildTime;
    public bool Built;

    protected void StartBuilding()
    {
        // Start gradually creating the building
        if (BuildProgress < 100)
        {
            InvokeRepeating("BuildProgressIncrease", 0, BuildTime / 100);
        }
    }

    public void FinishBuilding()
    {
        BuildProgress = 100;
        Built = true;
        CancelInvoke("BuildProgressIncrease");
    }

    protected void BuildProgressIncrease()
    {
        BuildProgress++;
        if (BuildProgress < 100) return;
        FinishBuilding();
    }

	// Use this for initialization
    public override void Start()
    {
        base.Start();

        SetEnabled(IsOwnedByActivePlayer());
        SetVisible(IsOwnedByActivePlayer());

        StartBuilding();
        if (BuildTime <= 0)
            FinishBuilding();
    }

    public bool IsOwnedByActivePlayer()
    {
        return GetOwner() == transform.root.GetComponent<Game>().ActivePlayer;
    }

    public GameObject GetOwner()
    {
        return transform.parent.parent.gameObject;
    }

}
