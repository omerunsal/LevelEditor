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
    // public List<Sector> Sectors { get; set; }

    public Level(int _levelNumber, ObjectGroup[] _objectGroups)
    {
        this.LevelNumber = _levelNumber;
        this.ObjectGroups = _objectGroups;
        // Sectors = _sectors;
    }
    
    public string ToJson()
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ContractResolver = new MyJsonContractResolver()
        };

        return JsonConvert.SerializeObject(this, settings);
    }

    public static Level FromJson(string jsonString)
    {
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new MyJsonContractResolver()
        };

        return JsonConvert.DeserializeObject<Level>(jsonString, settings);
    }
    
    public static List<Level> ListFromJson(string jsonString)
    {
        // var settings = new JsonSerializerSettings
        // {
        //     ContractResolver = new MyJsonContractResolver()
        // };
        //
        // return JsonConvert.DeserializeObject<List<Level>>(jsonString, settings);
        
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new MyJsonContractResolver()
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
        
        // var settings = new JsonSerializerSettings
        // {
        //     ContractResolver = new MyJsonContractResolver()
        // };
        //
        // var result = JsonConvert.DeserializeObject<List<Level>>(jsonString, settings);
        //
        // if (result == null || result.Count == 0)
        // {
        //     throw new JsonSerializationException("Deserialization failed or empty JSON string");
        // }
        //
        // return result;
    }
}