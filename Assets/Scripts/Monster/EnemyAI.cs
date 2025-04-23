using Assets.Scripts.StateMachines;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    //public float wanderRadius = 10f;
    //public float wanderTimer = 5f;

    //private NavMeshAgent agent;
    //private float timer;

    //public float wanderSpeed = 2f;
    //public float chaseSpeed = 4f;

    //public Transform player;
    //public float detectionRange = 10f;
    //private bool isChasing = false;

    public float fieldOfView = 90f; 
    public LayerMask detectionMask; 

    public Transform Player;
    public float DetectionRange = 10f;
    public float AttackRange = 3f;
    public float WanderRadius = 10f;
    public float WanderTimer = 5f;
    public float ChaseSpeed =4.5f;
    public float WanderSpeed = 2f;
    public Animator Animator;
    public NavMeshAgent Agent;

    private EnemyState currentState;
    public float wanderTimerElapsed;

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {

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

    public bool CanSeePlayer()
    {
        Vector3 dirToPlayer = (Player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, Player.position);

        if (distance > DetectionRange)
            return false;

        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle > fieldOfView / 2f)
            return false;

        if (Physics.Raycast(transform.position + Vector3.up, dirToPlayer, out RaycastHit hit, DetectionRange, detectionMask))
        {
            return hit.transform == Player;
        }

        return false;
    }
}