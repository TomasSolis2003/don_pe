using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class IaDuende : MonoBehaviour
{
    [Header("Movimiento")]
    public float wanderRadius = 8f;           // Radio máximo donde patrulla
    public float wanderDelay = 3f;            // Tiempo de espera entre puntos

    [Header("Detección y ataque")]
    public float detectionRadius = 10f;       // Rango para ver al jugador
    public float attackRange = 1.5f;          // Rango de ataque
    public int contactDamage = 10;            // Daño
    public float attackCooldown = 1.2f;       // Cooldown ataque

    [Header("Vida")]
    public int maxHealth = 50;

    [Header("Referencias")]
    public string playerTag = "Player";

    private int currentHealth;
    private NavMeshAgent agent;
    private Transform player;
    private float lastAttackTime = -999f;
    private bool isChasing = false;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private Vector3 wanderTarget;
    private Vector3 lastPlayerPos;

    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();

        GameObject p = GameObject.FindGameObjectWithTag(playerTag);
        if (p != null)
            player = p.transform;

        ChooseNewWanderTarget();
    }

    void Update()
    {
        if (player == null) return;

        DetectPlayer();

        if (isChasing)
            ChasePlayer();
        else
            Wander();
    }

    // ----------------------------
    // DETECCIÓN DEL JUGADOR
    // ----------------------------
    void DetectPlayer()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= detectionRadius)
        {
            if (!isChasing)
            {
                isChasing = true;
                isWaiting = false;
                agent.isStopped = false;
            }
        }
        else if (dist > detectionRadius * 1.3f)
        {
            if (isChasing)
            {
                isChasing = false;
                ChooseNewWanderTarget();
            }
        }
    }

    // ----------------------------
    // PERSEGUIR AL JUGADOR
    // ----------------------------
    void ChasePlayer()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        // Solo recalcula destino si el jugador se movió lo suficiente
        if (Vector3.Distance(player.position, lastPlayerPos) > 0.5f)
        {
            agent.SetDestination(player.position);
            lastPlayerPos = player.position;
        }

        // Atacar si está cerca
        if (distToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            DealDamage();
            lastAttackTime = Time.time;
        }
    }

    // ----------------------------
    // MOVIMIENTO ALEATORIO
    // ----------------------------
    void Wander()
    {
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                ChooseNewWanderTarget();
            }
            return;
        }

        // Si llegó al destino, esperar un poco antes de elegir otro
        if (!agent.pathPending && agent.remainingDistance <= 0.5f)
        {
            isWaiting = true;
            waitTimer = wanderDelay;
        }
    }

    void ChooseNewWanderTarget()
    {
        Vector3 randomDir = Random.insideUnitSphere * wanderRadius;
        randomDir += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDir, out hit, wanderRadius, NavMesh.AllAreas))
        {
            wanderTarget = hit.position;
            agent.SetDestination(wanderTarget);
        }
    }

    // ----------------------------
    // DAÑO Y VIDA
    // ----------------------------
    void DealDamage()
    {
        if (player == null) return;

        // Si tenés un script PlayerHealth:
        player.GetComponent<PlayerHealth>()?.RecibirDaño(contactDamage);
        Debug.Log($"{name} hizo {contactDamage} de daño al jugador");
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }

    // ----------------------------
    // DEBUG VISUAL
    // ----------------------------
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
