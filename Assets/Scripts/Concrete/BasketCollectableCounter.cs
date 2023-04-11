using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketCollectableCounter : MonoBehaviour
{
    public int CollectedCount;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collectable>())
        {
            CollectedCount++;
            Destroy(other);
        }
    }
}
