using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class Level
{
    public int LevelNumber { get; set; }
    public ObjectGroup[] ObjectGroups { get; set; }
    public int FirstCheckpointCount { get; set; }
    public int SecondCheckpointCount { get; set; }

    public Level(int _levelNumber, ObjectGroup[] _objectGroups,  int _firstCheckpointCount, int _secondCheckpointCount )
    {
        this.LevelNumber = _levelNumber;
        this.ObjectGroups = _objectGroups;
        this.FirstCheckpointCount = _firstCheckpointCount;
        this.SecondCheckpointCount = SecondCheckpointCount;
    }
    
    public string ToJson()
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ContractResolver = new JsonContractResolver()
        };

        return JsonConvert.SerializeObject(this, settings);
    }

    public static Level FromJson(string jsonString)
    {
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new JsonContractResolver()
        };

        return JsonConvert.DeserializeObject<Level>(jsonString, settings);
    }
    
    public static List<Level> ListFromJson(string jsonString)
    {
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new JsonContractResolver()
        };
        
        try
        {
            var level = JsonConvert.DeserializeObject<Level>(jsonString, settings);
            return new List<Level> { level };
        }
        catch (JsonSerializationException)
        {
            return JsonConvert.DeserializeObject<List<Level>>(jsonString, settings);
        }
        
    }
}