using Assets.Scripts.Monster.StateMachines;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.StateMachines
{
    public class ChaseState : EnemyState
    {
        private float roarDelayTimer = 0.5f; // half a second delay
        private bool hasRoared = false;
        public ChaseState(EnemyAI enemy) : base(enemy) { }


        public override void Enter()
        {

            enemy.Agent.speed = enemy.ChaseSpeed;
            enemy.Animator.SetBool("IsChasing", true);
            enemy.Animator.SetBool("IsWalking", false);

            roarDelayTimer = 0.5f;
            hasRoared = false;
        }

        public override void Update()
        {
            enemy.Agent.SetDestination(enemy.Player.position);

            if (!hasRoared)
            {
                roarDelayTimer -= Time.deltaTime;
                if (roarDelayTimer <= 0f)
                {
                    PlayRoar();
                    hasRoared = true;
                    CameraShake.Instance.Shake(0.4f, 0.2f);
                }
            }


            if (enemy.Agent.pathStatus == NavMeshPathStatus.PathPartial)
            {
                Debug.Log("Path blocked — switching to SearchState");
                enemy.LastKnownPosition = enemy.Player.position;
                enemy.ChangeState(new SearchState(enemy));
            }


            if (PlayerEscaped())
            {
                enemy.LastKnownPosition = enemy.Player.position;
                Debug.Log("Player Escaped");
                enemy.ChangeState(new SearchState(enemy));

            }

            if (CaughtPlayer())
            {
                Debug.Log("Got the player!");

                enemy.ChangeState(new KillPlayerState(enemy));
            }
        }

        private void PlayRoar()
        {
            if (enemy.RoarClip != null && enemy.AudioSource != null)
            {
                float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.position);
                float volume = Mathf.Clamp01(1f - (distanceToPlayer / enemy.DetectionRange));

                enemy.AudioSource.pitch = Random.Range(0.9f, 1.1f);
                enemy.AudioSource.PlayOneShot(enemy.RoarClip, volume);

                Debug.Log("Roar!");
            }
        }

        private bool CaughtPlayer()
        {
            return Vector3.Distance(enemy.transform.position, enemy.Player.position) < enemy.AttackRange;
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