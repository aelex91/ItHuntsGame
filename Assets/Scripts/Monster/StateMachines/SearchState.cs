using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts.StateMachines
{
    public class SearchState : EnemyState
    {
        private float searchTimer = 0f;
        private float maxSearchTime = 5f; // seconds

        public SearchState(EnemyAI enemy) : base(enemy) { }

        public override void Enter()
        {
            enemy.Agent.speed = enemy.WanderSpeed;
            enemy.Agent.SetDestination(enemy.LastKnownPosition);
            enemy.Animator.SetBool("IsWalking", true);
            Debug.Log("Searching last known position...");
        }

        public override void Update()
        {
            if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance <= 0.5f)
            {
                searchTimer += Time.deltaTime;

                if (enemy.PlayerController.IsRunningInsideEnemyDetectionRange(enemy)){

                    Debug.Log("Player is running inside detection range!!");
                    enemy.ChangeState(new ChaseState(enemy));
                    return;
                }

                if (searchTimer >= maxSearchTime)
                {
                    enemy.ChangeState(new WanderState(enemy));
                    return;
                }
            }

            // Player found after searching
            if (enemy.transform.InsideRadius(enemy.Player, enemy.DetectionRange))
            {
                if (enemy.CanSeePlayer() == false)
                    return;

                Debug.Log("In Searchstate -> But player is inside detectionrange");
                enemy.ChangeState(new ChaseState(enemy));
            }
        }

        public override void Exit()
        {
            enemy.Animator.SetBool("IsWalking", false);
        }
    }
}

