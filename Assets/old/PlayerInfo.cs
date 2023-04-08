using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
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
    }
}