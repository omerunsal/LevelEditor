using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class ObjectGroup
{
    public Shape ObjectShape { get; set; }
    public Vector3 Position { get; set; }
    public Vector3 Rotation { get; set; }
    public GroupLayout GroupLayout { get; set; }

    public ObjectGroup(Shape _objectShape, Vector3 _position, Vector3 _rotation, GroupLayout _groupLayout)
    {
        ObjectShape = _objectShape;
        Position = _position;
        Rotation = _rotation;
        GroupLayout = _groupLayout;
    }

    public string ToJson()
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ContractResolver = new MyJsonContractResolver()
        };

        return JsonConvert.SerializeObject(this, settings);
    }

    public static ObjectGroup FromJson(string jsonString)
    {
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new MyJsonContractResolver()
        };

        return JsonConvert.DeserializeObject<ObjectGroup>(jsonString, settings);
    }
}


public enum Shape
{
    Cube,
    Sphere
}

public enum GroupLayout
{
    Square,
    Triangle,
    X
}