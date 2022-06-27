using UnityEngine;

namespace Views.Helpers
{
    public class StandardInput
    {
        public bool Touch() => Input.GetMouseButtonDown(0);
        public bool Cancel() => Input.GetMouseButtonDown(1);
        public Vector3 Point() => Input.mousePosition;
    }
}
