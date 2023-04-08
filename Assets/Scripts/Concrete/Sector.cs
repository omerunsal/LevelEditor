using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector 
{
    public Vector3 Position { get; set; }
    public Sector SectorType { get; set; }
    public int SectorCount { get; set; }

    public Sector(Vector3 _position, Sector _sectorType,int _sectorCount)
    {
        Position = _position;
        SectorType = _sectorType;
        SectorCount = _sectorCount;
    }
}

public enum SectorType
{
    Gate,
    Normal,
    Finish
}