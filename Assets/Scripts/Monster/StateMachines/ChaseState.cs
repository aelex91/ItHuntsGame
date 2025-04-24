using Assets.Scripts.Monster.StateMachines;
using UnityEngine;

namespace Assets.Scripts.StateMachines
{
    public class ChaseState : EnemyState
    {
        public ChaseState(EnemyAI enemy) : base(enemy) { }

        public override void Enter()
        {
            enemy.Agent.speed = enemy.ChaseSpeed;
            enemy.Animator.SetBool("IsChasing", true);
            enemy.Animator.SetBool("IsWalking", false);
            Debug.Log("Player spotted — chasing!");
        }

        public override void Update()
        {
            enemy.Agent.SetDestination(enemy.Player.position);

            if (PlayerEscaped())
            {
                Debug.Log("Player Escaped");
                enemy.ChangeState(new WanderState(enemy));

            }

            if (CaughtPlayer())
            {
                Debug.Log("Got the player!");

                enemy.ChangeState(new KillPlayerState(enemy));
            }
        }

        private bool CaughtPlayer()
        {
            return Vector3.Distance(enemy.transform.position, enemy.Player.position) > enemy.AttackRange;
        }

        private bool PlayerEscaped()
        {
            
            return Vector3.Distance(enemy.transform.position, enemy.Player.position) > enemy.DetectionRange;
        }

        public override void Exit()
        {
            enemy.Animator.SetBool("IsChasing", false);
        }
    }
}