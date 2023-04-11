using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JsonContractResolver : DefaultContractResolver
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

