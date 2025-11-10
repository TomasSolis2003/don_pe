/*using UnityEngine;
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
*/
/*using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIA_Duende : MonoBehaviour, IDañoRecibible
{
    private enum Estado { Buscando, YendoObjetivo, Investigando, PersiguiendoJugador, Patrullando, Huyendo }

    [Header("Configuración general")]
    public bool esCazadorDeJugador = false; // si true, ignora objetos y busca al jugador
    public GameObject prefabTierraSinPreparar;
    public Transform jugadorManual;
    public Transform[] viasDeEscape; // se asigna automáticamente si no tiene
    public LayerMask capasTerreno;
    public string tagTrigo = "trigo";
    public string tagHoguera = "Hoguera";
    public string tagLobo = "LoboSalvaje";

    [Header("Parámetros de IA")]
    public float radioDeteccion = 20f;
    public float tiempoBusqueda = 5f;
    public float tiempoInvestigacion = 20f;
    public float tiempoPatrulla = 15f;
    public float radioMerodeo = 20f;
    public float distanciaAccion = 1.5f;
    public float velocidadNormal = 3.5f;
    public float velocidadHuida = 9f;

    [Header("Combate")]
    public int vidaMaxima = 20;
    public int dañoBase = 10;
    public float cdAtaque = 1.2f;
    public float rangoAtaque = 1.7f;

    private int vidaActual;
    private NavMeshAgent agent;
    private Estado estado = Estado.Buscando;
    private GameObject objetivoActual;
    private Transform objetivoJugador;
    private bool estaVivo = true;
    private bool puedeAtacar = true;
    private Vector3 puntoOrigen;
    private AudioSource audioSrc;
    private bool activo = true;

    void OnEnable() => SunMovement.OnCambioDiaNoche += OnCambioDiaNoche;
    void OnDisable() => SunMovement.OnCambioDiaNoche -= OnCambioDiaNoche;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        vidaActual = vidaMaxima;
        puntoOrigen = transform.position;
        audioSrc = GetComponent<AudioSource>();

        // Autoasignaciones
        if (jugadorManual == null && GameObject.FindGameObjectWithTag("Player"))
            jugadorManual = GameObject.FindGameObjectWithTag("Player").transform;

        if (viasDeEscape == null || viasDeEscape.Length == 0)
        {
            GameObject[] posibles = GameObject.FindGameObjectsWithTag("Escape");
            viasDeEscape = new Transform[posibles.Length];
            for (int i = 0; i < posibles.Length; i++) viasDeEscape[i] = posibles[i].transform;
        }

        if (prefabTierraSinPreparar == null)
        {
            GameObject refTierra = GameObject.Find("TierraSinPrepararPrefab");
            if (refTierra != null) prefabTierraSinPreparar = refTierra;
        }
    }


    void OnCambioDiaNoche(bool esDia)
    {
        activo = !esDia;
        if (agent != null) agent.isStopped = esDia;
        if (esDia) { estado = Estado.Buscando; objetivoActual = null; }
    }

    void Update()
    {
        if (!estaVivo || !activo) return;

        switch (estado)
        {
            case Estado.Buscando:
                if (esCazadorDeJugador) BuscarJugador();
                else BuscarObjetivo();
                break;

            case Estado.YendoObjetivo:
                if (!objetivoActual)
                {
                    estado = Estado.Buscando;
                    return;
                }
                agent.SetDestination(objetivoActual.transform.position);
                if (Vector3.Distance(transform.position, objetivoActual.transform.position) <= distanciaAccion)
                    StartCoroutine(InvestigarObjetivo());
                break;

            case Estado.Investigando:
                break;

            case Estado.PersiguiendoJugador:
                if (objetivoJugador == null) { estado = Estado.Buscando; break; }
                agent.SetDestination(objetivoJugador.position);
                if (Vector3.Distance(transform.position, objetivoJugador.position) <= rangoAtaque && puedeAtacar)
                    StartCoroutine(AtaqueJugadorCR());
                break;

            case Estado.Patrullando:
                break;

            case Estado.Huyendo:
                break;
        }
    }

    // --------------------------- BUSQUEDA ---------------------------

    void BuscarJugador()
    {
        if (jugadorManual == null) return;
        objetivoJugador = jugadorManual;
        estado = Estado.PersiguiendoJugador;
        agent.speed = velocidadNormal * 1.3f;
    }

    void BuscarObjetivo()
    {
        GameObject[] posibles = GameObject.FindGameObjectsWithTag(tagTrigo);
        if (posibles.Length == 0) posibles = GameObject.FindGameObjectsWithTag(tagHoguera);
        if (posibles.Length == 0) posibles = GameObject.FindGameObjectsWithTag(tagLobo);

        if (posibles.Length > 0)
        {
            objetivoActual = posibles[Random.Range(0, posibles.Length)];
            estado = Estado.YendoObjetivo;
            agent.speed = velocidadNormal;
        }
        else
        {
            StartCoroutine(ModoExploracion());
        }
    }

    IEnumerator ModoExploracion()
    {
        estado = Estado.Patrullando;
        Vector3 randomDir = Random.insideUnitSphere * radioMerodeo; randomDir.y = 0;
        Vector3 destino = puntoOrigen + randomDir;

        if (NavMesh.SamplePosition(destino, out NavMeshHit hit, radioMerodeo, NavMesh.AllAreas))
            agent.SetDestination(hit.position);

        yield return new WaitForSeconds(tiempoPatrulla);

        estado = Estado.Buscando;
    }

    IEnumerator InvestigarObjetivo()
    {
        estado = Estado.Investigando;
        agent.ResetPath();

        yield return new WaitForSeconds(tiempoInvestigacion);

        // Vuelve a modo patrulla
        estado = Estado.Patrullando;
        StartCoroutine(ModoExploracion());
    }

    IEnumerator AtaqueJugadorCR()
    {
        puedeAtacar = false;
        var vida = objetivoJugador.GetComponent<IDañoRecibible>();
        if (vida != null)
            vida.RecibirDaño(dañoBase);

        yield return new WaitForSeconds(cdAtaque);
        puedeAtacar = true;
    }

    // --------------------------- DAÑO Y MUERTE ---------------------------

    public void RecibirDaño(int cantidad)
    {
        if (!estaVivo) return;

        vidaActual -= cantidad;
        if (vidaActual <= 0) Morir();
        else if (Random.value < 0.3f) IniciarHuida();
    }

    void Morir()
    {
        if (!estaVivo) return;
        estaVivo = false;
        Destroy(gameObject);
    }

    void IniciarHuida()
    {
        if (viasDeEscape == null || viasDeEscape.Length == 0) return;

        estado = Estado.Huyendo;
        Transform destino = viasDeEscape[Random.Range(0, viasDeEscape.Length)];
        agent.speed = velocidadHuida;
        agent.SetDestination(destino.position);
        StartCoroutine(DesaparecerTrasTiempo(10f));
    }

    IEnumerator DesaparecerTrasTiempo(float t)
    {
        yield return new WaitForSeconds(t);
        if (estaVivo) Destroy(gameObject);
    }

    // --------------------------- GIZMOS ---------------------------
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioMerodeo);
    }
}
*/
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIA_Duende : MonoBehaviour, IDañoRecibible
{
    private enum Estado { Buscando, YendoObjetivo, Investigando, PersiguiendoJugador, Patrullando, Huyendo }

    [Header("Configuración general")]
    public bool esCazadorDeJugador = false;
    public GameObject prefabTierraSinPreparar;
    public Transform jugadorManual;
    public Transform[] viasDeEscape;

    [Header("Tags globales")]
    public string tagJugador = "Player";
    public string tagEscape = "Escape";
    public string tagTierra = "TierraSinPrepararPrefab";

    [Header("Parámetros de IA")]
    public float radioDeteccion = 20f;
    public float tiempoBusqueda = 5f;
    public float tiempoInvestigacion = 20f;
    public float tiempoPatrulla = 15f;
    public float radioMerodeo = 20f;
    public float distanciaAccion = 1.5f;
    public float velocidadNormal = 3.5f;
    public float velocidadHuida = 9f;

    [Header("Combate")]
    public int vidaMaxima = 20;
    public int dañoBase = 10;
    public float cdAtaque = 1.2f;
    public float rangoAtaque = 1.7f;

    private int vidaActual;
    private NavMeshAgent agent;
    private Estado estado = Estado.Buscando;
    private GameObject objetivoActual;
    private Transform objetivoJugador;
    private bool estaVivo = true;
    private bool puedeAtacar = true;
    private Vector3 puntoOrigen;
    private AudioSource audioSrc;
    private bool activo = true;

    void OnEnable() => SunMovement.OnCambioDiaNoche += OnCambioDiaNoche;
    void OnDisable() => SunMovement.OnCambioDiaNoche -= OnCambioDiaNoche;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        vidaActual = vidaMaxima;
        puntoOrigen = transform.position;
        audioSrc = GetComponent<AudioSource>();

        // ✅ Buscar por tag automáticamente si no tiene referencias
        if (jugadorManual == null)
        {
            GameObject j = GameObject.FindGameObjectWithTag(tagJugador);
            if (j != null) jugadorManual = j.transform;
        }

        if ((viasDeEscape == null || viasDeEscape.Length == 0))
        {
            GameObject[] escapes = GameObject.FindGameObjectsWithTag(tagEscape);
            viasDeEscape = new Transform[escapes.Length];
            for (int i = 0; i < escapes.Length; i++) viasDeEscape[i] = escapes[i].transform;
        }

        if (prefabTierraSinPreparar == null)
        {
            GameObject tierra = GameObject.FindGameObjectWithTag(tagTierra);
            if (tierra != null) prefabTierraSinPreparar = tierra;
        }
    }

    void OnCambioDiaNoche(bool esDia)
    {
        activo = !esDia;
        if (agent != null) agent.isStopped = esDia;
        if (esDia) { estado = Estado.Buscando; objetivoActual = null; }
    }

    void Update()
    {
        if (!estaVivo || !activo) return;

        switch (estado)
        {
            case Estado.Buscando:
                if (esCazadorDeJugador) BuscarJugador();
                else BuscarObjetivo();
                break;

            case Estado.YendoObjetivo:
                if (!objetivoActual) { estado = Estado.Buscando; return; }
                agent.SetDestination(objetivoActual.transform.position);
                if (Vector3.Distance(transform.position, objetivoActual.transform.position) <= distanciaAccion)
                    StartCoroutine(InvestigarObjetivo());
                break;

            case Estado.PersiguiendoJugador:
                if (objetivoJugador == null) { estado = Estado.Buscando; break; }
                agent.SetDestination(objetivoJugador.position);
                if (Vector3.Distance(transform.position, objetivoJugador.position) <= rangoAtaque && puedeAtacar)
                    StartCoroutine(AtaqueJugadorCR());
                break;

            case Estado.Investigando:
            case Estado.Patrullando:
            case Estado.Huyendo:
                break;
        }
    }

    // --------------------------- BUSQUEDA ---------------------------
    void BuscarJugador()
    {
        if (jugadorManual == null) return;
        objetivoJugador = jugadorManual;
        estado = Estado.PersiguiendoJugador;
        agent.speed = velocidadNormal * 1.3f;
    }

    void BuscarObjetivo()
    {
        GameObject[] posibles = GameObject.FindGameObjectsWithTag("trigo");
        if (posibles.Length == 0) posibles = GameObject.FindGameObjectsWithTag("Hoguera");
        if (posibles.Length == 0) posibles = GameObject.FindGameObjectsWithTag("LoboSalvaje");

        if (posibles.Length > 0)
        {
            objetivoActual = posibles[Random.Range(0, posibles.Length)];
            estado = Estado.YendoObjetivo;
            agent.speed = velocidadNormal;
        }
        else
        {
            StartCoroutine(ModoExploracion());
        }
    }

    IEnumerator ModoExploracion()
    {
        estado = Estado.Patrullando;
        Vector3 randomDir = Random.insideUnitSphere * radioMerodeo; randomDir.y = 0;
        Vector3 destino = puntoOrigen + randomDir;

        if (NavMesh.SamplePosition(destino, out NavMeshHit hit, radioMerodeo, NavMesh.AllAreas))
            agent.SetDestination(hit.position);

        yield return new WaitForSeconds(tiempoPatrulla);
        estado = Estado.Buscando;
    }

    IEnumerator InvestigarObjetivo()
    {
        estado = Estado.Investigando;
        agent.ResetPath();
        yield return new WaitForSeconds(tiempoInvestigacion);
        estado = Estado.Patrullando;
        StartCoroutine(ModoExploracion());
    }

    IEnumerator AtaqueJugadorCR()
    {
        puedeAtacar = false;
        var vida = objetivoJugador.GetComponent<IDañoRecibible>();
        if (vida != null)
            vida.RecibirDaño(dañoBase);

        yield return new WaitForSeconds(cdAtaque);
        puedeAtacar = true;
    }

    // --------------------------- DAÑO ---------------------------
    public void RecibirDaño(int cantidad)
    {
        if (!estaVivo) return;

        vidaActual -= cantidad;
        if (vidaActual <= 0) Morir();
        else if (Random.value < 0.3f) IniciarHuida();
    }

    void Morir()
    {
        if (!estaVivo) return;
        estaVivo = false;
        Destroy(gameObject);
    }

    void IniciarHuida()
    {
        if (viasDeEscape == null || viasDeEscape.Length == 0) return;

        estado = Estado.Huyendo;
        Transform destino = viasDeEscape[Random.Range(0, viasDeEscape.Length)];
        agent.speed = velocidadHuida;
        agent.SetDestination(destino.position);
        StartCoroutine(DesaparecerTrasTiempo(10f));
    }

    IEnumerator DesaparecerTrasTiempo(float t)
    {
        yield return new WaitForSeconds(t);
        if (estaVivo) Destroy(gameObject);
    }
}
