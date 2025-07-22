using UnityEngine;
using UnityEngine.AI;

public class AIEnemy : MonoBehaviour
{
    [Header("Patrol")]
    public Transform[] waypoints;
    public float patrolSpeed = 3f;

    [Header("Chase/Attack")]
    public float chaseSpeed = 5f;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float timeBetweenAttacks = 1f;

    private NavMeshAgent agent;
    private Transform player;
    private int currentWaypoint = 0;
    private float attackTimer = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        if (waypoints.Length > 0)
        {
            agent.speed = patrolSpeed;
            agent.SetDestination(waypoints[currentWaypoint].position);
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        agent.speed = patrolSpeed;
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypoint].position);
        }
    }

    void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        agent.ResetPath();
        attackTimer += Time.deltaTime;
        if (attackTimer >= timeBetweenAttacks)
        {
            // Aquí implementar daño u otra lógica de ataque
            Debug.Log("Enemy attacks the player!");
            attackTimer = 0f;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
