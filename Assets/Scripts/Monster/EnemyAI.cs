using Assets.Scripts.Managers;
using Assets.Scripts.Monster.StateMachines;
using Assets.Scripts.StateMachines;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public AudioSource AudioSource;
    public AudioClip RoarClip;

    public Vector3 LastKnownPosition;

    public float fieldOfView = 90f;
    public LayerMask detectionMask;

    public Transform Player;
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
        Vector3 dirToPlayer = (Player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, Player.position);

        if (distance > DetectionRange)
            return false;

        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle > fieldOfView / 2f)
            return false;

        Debug.DrawRay(transform.position + Vector3.up, dirToPlayer * DetectionRange, Color.red);
        if (Physics.SphereCast(transform.position + Vector3.up, 0.5f, dirToPlayer, out RaycastHit hit, DetectionRange, detectionMask))
        {
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