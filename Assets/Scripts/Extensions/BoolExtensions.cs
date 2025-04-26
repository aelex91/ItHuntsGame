using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class BoolExtensions
    {
        public static bool InsideRadius(this Transform transform, Transform other, float detectionRange)
        {
            var dirToPlayer = (other.position - transform.position).normalized;
            var distance = Vector3.Distance(transform.position, other.position);

            return distance <= detectionRange;
        }

        public static bool IsRunningInsideEnemyDetectionRange(this PlayerController player, EnemyAI enemy)
        {
            if (player.transform.InsideRadius(enemy.transform, enemy.DetectionRange) == false)
                return false;

            return player.MovementType == Enums.MovementType.Running;

        }
    }
}
