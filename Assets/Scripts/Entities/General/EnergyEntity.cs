using UnityEngine;
using System.Collections;

public class EnergyEntity : Entity {

    public int EnergyValue;

    public override void SetVisible(bool visibleValue)
    {
        Visible = visibleValue;
        GetComponent<ParticleSystem>().renderer.enabled = visibleValue;
        SetEnabled(visibleValue);
    }
    public override void SetEnabled(bool enabledValue)
    {
    }
}
