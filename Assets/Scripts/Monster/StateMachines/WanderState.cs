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
            Debug.Log("Lost sight of player — resuming wander.");
        }

        public override void Update()
        {

            CheckPlayerBehind();

            Vector3 toPlayer = (enemy.Player.position - enemy.transform.position).normalized;
            float angle = Vector3.Angle(enemy.transform.forward, toPlayer);
            float distance = Vector3.Distance(enemy.transform.position, enemy.Player.position);

            bool canSee = enemy.CanSeePlayer();

            if (canSee && angle > (enemy.fieldOfView / 2f) * 0.7f) // edge of vision
            {
                enemy.ChaseTimer += Time.deltaTime;

                if (enemy.ChaseTimer >= enemy.timeBeforeChase)
                {
                    enemy.LastKnownPosition = enemy.Player.position;
                    enemy.ChangeState(new ChaseState(enemy));
                    enemy.ChaseTimer = 0f;
                }
            }
            else if (canSee) // clearly in front
            {
                enemy.LastKnownPosition = enemy.Player.position;
                enemy.ChangeState(new ChaseState(enemy));
                enemy.ChaseTimer = 0f;
            }
            else
            {
                enemy.ChaseTimer = 0f;
            }

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
            Vector3 toPlayer = (enemy.Player.position - enemy.transform.position).normalized;
            float angleToPlayer = Vector3.Angle(enemy.transform.forward, toPlayer);
            float distanceToPlayer = Vector3.Distance(enemy.Player.position, enemy.transform.position);

            if (angleToPlayer > 120f && distanceToPlayer < 2f)
            {
                enemy.LastKnownPosition = enemy.Player.position;
                enemy.ChangeState(new ChaseState(enemy));
            }
        }

    }
}
