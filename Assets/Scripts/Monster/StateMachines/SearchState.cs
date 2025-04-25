using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                if (searchTimer >= maxSearchTime)
                {
                    enemy.ChangeState(new WanderState(enemy));
                }
            }

            // Player found again
            if (Vector3.Distance(enemy.transform.position, enemy.Player.position) < enemy.DetectionRange)
            {
                enemy.ChangeState(new ChaseState(enemy));
            }
        }

        public override void Exit()
        {
            enemy.Animator.SetBool("IsWalking", false);
        }
    }
}

