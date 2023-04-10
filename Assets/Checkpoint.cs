using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public int CheckpointCount;
    public TextMeshProUGUI CountText;

    private void Awake()
    {
        CountText.text = $"0 / " + CheckpointCount.ToString();
    }
}
