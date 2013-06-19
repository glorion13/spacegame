using UnityEngine;
using System.Collections;

public class LineOfSightBehaviour : MonoBehaviour {

    public int LineOfSightRadius;
    private InstanceBehaviour thisInstance;

    void Start()
    {
        thisInstance = this.gameObject.GetComponent<InstanceBehaviour>();
    }
}