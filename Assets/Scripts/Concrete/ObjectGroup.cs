using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

public enum Shape
{
    Cube,
    Sphere
}

public enum GroupLayout
{
    Square,
    Triange,
    X
}