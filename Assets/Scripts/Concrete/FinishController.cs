﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : MonoBehaviour
{
    private Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CompletedLevelSectorCount++;
            collider.enabled = false;
            GameManager.Instance.isLevelStarted = false;
            GameManager.Instance.LevelComplete();
        }
    }
}