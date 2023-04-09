using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level
{
    public int LevelNumber { get; set; }
    public List<ObjectGroup> ObjectGroups { get; set; }
    // public List<Sector> Sectors { get; set; }

    public Level(int _levelNumber, List<ObjectGroup> _objectGroups)
    {
        LevelNumber = _levelNumber;
        ObjectGroups = _objectGroups;
        // Sectors = _sectors;
    }
}