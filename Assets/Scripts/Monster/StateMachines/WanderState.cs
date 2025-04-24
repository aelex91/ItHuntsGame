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
            if (enemy.CanSeePlayer())
            {
                enemy.ChangeState(new ChaseState(enemy));
                return;
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


    }
}
