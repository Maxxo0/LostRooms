using UnityEngine;
using UnityEngine.AI;

public class AIEnemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float wanderRadius = 20f;
    public float wanderTimer = 5f;

    [Header("Vision Settings")]
    public float visionRange = 10f;
    [Range(0, 360)] public float visionAngle = 120f;
    public LayerMask obstructMask;

    [Header("Chase Settings")]
    public float chaseSpeed = 5f;
    public float patrolSpeed = 3f;

    private NavMeshAgent agent;
    private Transform player;
    private float timer;
    private bool playerDetected = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timer = wanderTimer;
    }

    void Update()
    {
        DetectPlayer();

        if (playerDetected)
            ChasePlayer();
        else
            Patrol();
    }

    void Patrol()
    {
        agent.speed = patrolSpeed;
        timer += Time.deltaTime;
        if (timer >= wanderTimer || !agent.hasPath)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius);
            agent.SetDestination(newPos);
            timer = 0f;
        }
    }

    void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    void DetectPlayer()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer <= visionRange &&
            Vector3.Angle(transform.forward, dirToPlayer) <= visionAngle / 2)
        {
            if (!Physics.Raycast(transform.position + Vector3.up * 1.5f, dirToPlayer, distToPlayer, obstructMask))
            {
                playerDetected = true;
                return;
            }
        }
        playerDetected = false;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randomDir = Random.insideUnitSphere * dist;
        randomDir += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDir, out navHit, dist, NavMesh.AllAreas);
        return navHit.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        Vector3 fwd = Quaternion.Euler(0, visionAngle / 2, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, fwd * visionRange);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -visionAngle / 2, 0) * transform.forward * visionRange);
    }
}
