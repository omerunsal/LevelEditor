using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketCollectableCount : MonoBehaviour
{

    public int CollectedCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collectable>())
        {
            CollectedCount++;
            Destroy(other);
        }
    }
}
