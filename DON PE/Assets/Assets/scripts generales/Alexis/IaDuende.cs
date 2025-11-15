using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class IaDuende : MonoBehaviour
{
    [Header("Movimiento")]
    public float wanderRadius = 8f;
    public float wanderDelay = 3f;

    [Header("Detección y ataque")]
    public float detectionRadius = 10f;
    public float attackRange = 1.5f;
    public int contactDamage = 10;
    public float attackCooldown = 1.2f;
    public float attackPauseTime = 0.6f;  // 🆕 Tiempo detenido tras atacar

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
    private bool isAttacking = false;  // 🆕 evita moverse durante el ataque

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

        if (isAttacking) return; // 🆕 no moverse mientras ataca

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

        if (Vector3.Distance(player.position, lastPlayerPos) > 0.5f)
        {
            agent.SetDestination(player.position);
            lastPlayerPos = player.position;
        }

        if (distToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            StartCoroutine(AttackPause());
            lastAttackTime = Time.time;
        }
    }

    // 🆕 CORUTINA DE ATAQUE CON PAUSA
    IEnumerator AttackPause()
    {
        isAttacking = true;
        agent.isStopped = true;

        // Lógica de daño
        DealDamage();

        // Espera un poco antes de moverse de nuevo
        yield return new WaitForSeconds(attackPauseTime);

        agent.isStopped = false;
        isAttacking = false;
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hacha"))
        {
            // Obtener el script del hacha
            HachaA hacha = other.GetComponent<HachaA>();

            // Si el hacha existe y está atacando → hace daño al duende
            if (hacha != null && hacha.isAttacking)
            {
                TakeDamage(hacha.hitDamage); // usa el daño del hacha
                Debug.Log("Duende recibió daño del hacha");
            }
        }
    }
   
    // ----------------------------
    // DAÑO Y VIDA
    // ----------------------------
    void DealDamage()
    {
        if (player == null) return;

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