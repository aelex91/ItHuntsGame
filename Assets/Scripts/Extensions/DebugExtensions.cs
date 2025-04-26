using UnityEngine;
using Color = UnityEngine.Color;

namespace Assets.Scripts.Extensions
{
    public static class DebugExtensions
    {
        public static void DebugWireSphere(this Vector3 position, float radius, Color color, float duration = 0f)
        {
        #if UNITY_EDITOR
            UnityEngine.Debug.DrawLine(position + Vector3.up * radius, position - Vector3.up * radius, color, duration);
            UnityEngine.Debug.DrawLine(position + Vector3.right * radius, position - Vector3.right * radius, color, duration);
            UnityEngine.Debug.DrawLine(position + Vector3.forward * radius, position - Vector3.forward * radius, color, duration);
        #endif
        }

    }
}
