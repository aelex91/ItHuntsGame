using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts.StateMachines
{
    public class WanderState : EnemyState
    {
        public WanderState(EnemyAI enemy) : base(enemy) { }

        public override void Enter()
        {
            enemy.Agent.speed = enemy.WanderSpeed;
            enemy.ResetWanderTimer();
        }

        public override void Update()
        {
            //Om spelaren är inom detection range men bakom monstret (men movementtype är running, då bör monstret springa efter spelaren.)

            if (enemy.PlayerController.IsRunningInsideEnemyDetectionRange(enemy))
            {
                Debug.Log("Player is running inside detection range!!");
                enemy.ChangeState(new ChaseState(enemy));
                return;
            }

            CheckPlayerBehind();

            var toPlayer = (enemy.Player.position - enemy.transform.position).normalized;
            var angle = Vector3.Angle(enemy.transform.forward, toPlayer);
            var distance = Vector3.Distance(enemy.transform.position, enemy.Player.position);


            if (enemy.CanSeePlayer()) // clearly in front
            {
                Debug.Log("Can See Player -> Chasing");
                enemy.LastKnownPosition = enemy.Player.position;
                enemy.ChangeState(new ChaseState(enemy));

            }

            enemy.ChaseTimer = 0f;
            enemy.wanderTimerElapsed += Time.deltaTime;

            if (enemy.wanderTimerElapsed >= enemy.WanderTimer)
            {
                Vector3 newPos = EnemyAI.RandomNavSphere(enemy.transform.position, enemy.WanderRadius, -1);
                enemy.Agent.SetDestination(newPos);

                enemy.ResetWanderTimer();
                return;
            }

            bool isMoving = enemy.Agent.hasPath && enemy.Agent.remainingDistance > enemy.Agent.stoppingDistance;
            enemy.Animator.SetBool("IsWalking", isMoving);
        }

        private void CheckPlayerBehind()
        {
            var toPlayer = (enemy.Player.position - enemy.transform.position).normalized;
            var angleToPlayer = Vector3.Angle(enemy.transform.forward, toPlayer);
            var distanceToPlayer = Vector3.Distance(enemy.Player.position, enemy.transform.position);

            if (angleToPlayer > 120f && distanceToPlayer < 3f)
            {
                //Debug.Log("Player is behind me, lets catch him!");
                enemy.LastKnownPosition = enemy.Player.position;
                enemy.ChangeState(new ChaseState(enemy));
            }
        }

    }
}
