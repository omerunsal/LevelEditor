using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyJsonContractResolver : DefaultContractResolver
{
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);

        if (type == typeof(Vector3))
        {
            properties = properties.Where(p => p.PropertyName == "x" || p.PropertyName == "y" || p.PropertyName == "z").ToList();
        }

        return properties;
    }
}

public class PlayerInfo
{
    public Vector3 Position { get; set; }
    public Vector3 Rotation { get; set; }
    public string Name { get; set; }

    public PlayerInfo(Vector3 position, Vector3 rotation, string name)
    {
        Position = position;
        Rotation = rotation;
        Name = name;
    }

    public string ToJson()
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ContractResolver = new MyJsonContractResolver()
        };

        return JsonConvert.SerializeObject(this, settings);
    }
    
    public static PlayerInfo FromJson(string jsonString)
    {
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new MyJsonContractResolver()
        };

        return JsonConvert.DeserializeObject<PlayerInfo>(jsonString, settings);
    }
}