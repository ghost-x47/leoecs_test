using Logic.Components;
using UnityEngine;

namespace Views.Helpers
{
    public static class Vector3Extensions
    {
        public static Vector3 ToVec3(this Position position) => new Vector3(position.X, position.Y, position.Z);

        public static Position ToPos(this Vector3 vector3) =>
            new Position
            {
                X = vector3.x, Y = vector3.y, Z = vector3.z
            };
    }
}
