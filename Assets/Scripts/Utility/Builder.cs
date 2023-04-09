using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder<T> : MonoBehaviour where T : CollectableGroup
{
    public T BuildProductionWith(T buildUnit, Vector3 buildPosition)
    {
        return Instantiate(buildUnit, buildPosition, Quaternion.identity);
    }
    
    public GameObject BuildProduction(T buildUnit)
    {
        return Instantiate(buildUnit).gameObject;
    }
}
