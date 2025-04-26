using Assets.Scripts.Extensions;
using Assets.Scripts.Managers;
using Assets.Scripts.StateMachines;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public AudioSource AudioSource;
    public AudioClip RoarClip;

    public Vector3 LastKnownPosition;

    public float fieldOfView = 130f;
    public LayerMask detectionMask;

    public Transform Player;

    public PlayerController PlayerController;
    public float DetectionRange = 10f;

    public float AttackRange = 3f;
    public float WanderRadius = 10f;
    public float WanderTimer = 5f;
    public float ChaseSpeed = 4.5f;
    public float WanderSpeed = 2f;
    public Animator Animator;
    public NavMeshAgent Agent;

    private EnemyState currentState;
    public float wanderTimerElapsed;

    public float timeBeforeChase = 1.5f;
    public float ChaseTimer = 0f;

    public Transform[] SpawnPositions;

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        AudioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (PlayerController == null)
            PlayerController = Player.GetComponent<PlayerController>();

        SpawnPositions = GameManager.Instance.GetSpawnPositions();
        ChangeState(new WanderState(this));
    }

    void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(EnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void ResetWanderTimer()
    {
        wanderTimerElapsed = 0;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, DetectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }

    public bool CanSeePlayer()
    {
        var dirToPlayer = (Player.position - transform.position).normalized;

        var inside = this.transform.InsideRadius(Player.transform, DetectionRange);

        if (inside == false)
            return false;

        var angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle > fieldOfView / 2f)
            return false;

        if (Physics.SphereCast(transform.position + Vector3.up, 0.5f, dirToPlayer, out RaycastHit hit, DetectionRange, detectionMask))
        {
            Debug.Log($"SphereCast hit: {hit.transform.name} on layer {LayerMask.LayerToName(hit.transform.gameObject.layer)}");
            return hit.transform == Player;
        }

        return false;
    }

    public IEnumerator RotateTowards(Vector3 targetPos, System.Action onComplete = null)
    {
        Vector3 direction = (targetPos - transform.position);
        direction.y = 0f; // Ignore vertical difference
        if (direction == Vector3.zero)
        {
            onComplete?.Invoke();
            yield break;
        }

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        while (Quaternion.Angle(transform.rotation, lookRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            yield return null;
        }

        transform.rotation = lookRotation;
        onComplete?.Invoke();
    }

}