using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Player Setting", menuName = "PlayerSettings")]
public class Player : ScriptableObject
{
    public float moveSpeed;
    public float rotateSpeed;
    public Color collectorColor; 
}