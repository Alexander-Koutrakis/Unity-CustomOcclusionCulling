using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomOcclusion
{
    public struct Cube
    {
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;
        public readonly Vector3 Scale;

        public Cube(Vector3 position,Quaternion rotation ,Vector3 scale)
        {

            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
    }
}
