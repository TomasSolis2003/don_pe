/*using UnityEngine;
using UnityEngine.AI;

public class WolfAI : MonoBehaviour
{
    public enum Estado { Merodear, Perseguir, Volver }
    [Header("Refs")]
    public NavMeshAgent agent;
    public Transform objetivoManual;              // opcional: asigná el jugador acá
    [Tooltip("Capas que se consideran objetivos (ej. Player)")]
    public LayerMask capasObjetivo = ~0;
    [Tooltip("Capas que bloquean línea de visión (paredes/rocas)")]
    public LayerMask capasObstaculos;

    [Header("Detección")]
    public float radioDeteccion = 8f;
    public bool usarLineaVision = true;
    public float refrescoDeteccion = 0.2f;

    [Header("Movimiento")]
    public float radioMerodeo = 15f;      // radio alrededor del punto inicial
    public float umbralLlegada = 0.6f;    // cuándo consideramos que llegó
    public float intervaloNuevoDestino = 3f; // cada cuánto busca un nuevo punto si está en Merodear
    public float velocidadMerodeo = 2.2f;
    public float velocidadPersecucion = 4.0f;

    [Header("Combate (opcional)")]
    public float rangoAtaque = 1.8f;
    public float cdAtaque = 1.0f;
    public int dano = 10;

    [Header("Debug")]
    public bool dibujarGizmos = true;

    private Estado estado = Estado.Merodear;
    private Vector3 origen;              // punto base para merodear/volver
    private Transform objetivo;          // objetivo actual (normalmente el jugador)
    private float tSiguienteChequeo = 0f;
    private float tProximoDestino = 0f;
    private float tPuedeAtacar = 0f;

    void Reset()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Awake()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        origen = transform.position;
    }

    void OnEnable()
    {
        // arrancamos ya con un destino de merodeo
        SetEstado(Estado.Merodear);
        BuscarNuevoDestinoMerodeo(true);
    }

    void Update()
    {
        // 1) Detectar objetivo cada X segundos
        if (Time.time >= tSiguienteChequeo)
        {
            tSiguienteChequeo = Time.time + refrescoDeteccion;
            ActualizarDeteccion();
        }

        // 2) FSM simple
        switch (estado)
        {
            case Estado.Merodear:
                agent.speed = velocidadMerodeo;

                // si llegó al punto o pasó el intervalo, busca otro
                if (!agent.pathPending && agent.remainingDistance <= umbralLlegada
                    || Time.time >= tProximoDestino)
                {
                    BuscarNuevoDestinoMerodeo(false);
                }
                break;

            case Estado.Perseguir:
                agent.speed = velocidadPersecucion;
                if (objetivo)
                {
                    agent.SetDestination(objetivo.position);

                    float dist = Vector3.Distance(transform.position, objetivo.position);

                    // atacar si está cerca (opcional)
                    if (dist <= rangoAtaque)
                    {
                        IntentarAtacar();
                        // mantener presión rodeando levemente: no frenes en seco
                        Vector3 offset = (transform.position - objetivo.position).normalized * 0.5f;
                        agent.SetDestination(objetivo.position + offset);
                    }
                }
                else
                {
                    // perdimos el target → volver
                    SetEstado(Estado.Volver);
                }
                break;

            case Estado.Volver:
                agent.speed = velocidadMerodeo;
                agent.SetDestination(origen);

                if (!agent.pathPending && agent.remainingDistance <= umbralLlegada)
                {
                    SetEstado(Estado.Merodear);
                    BuscarNuevoDestinoMerodeo(true);
                }
                break;
        }
    }

    // ------------------- LÓGICA -------------------

    void ActualizarDeteccion()
    {
        // Si ya tenemos asignado uno manualmente, lo usamos
        if (objetivoManual != null)
        {
            objetivo = objetivoManual;
        }
        else
        {
            // Buscar el objetivo válido más cercano en radio
            Collider[] hits = Physics.OverlapSphere(transform.position, radioDeteccion, capasObjetivo, QueryTriggerInteraction.Ignore);
            Transform mejor = null; float mejorDist = float.MaxValue;

            foreach (var h in hits)
            {
                Transform t = h.attachedRigidbody ? h.attachedRigidbody.transform : h.transform;
                float d = Vector3.SqrMagnitude(t.position - transform.position);
                if (d < mejorDist)
                {
                    // opcional: chequear línea de visión
                    if (!usarLineaVision || TieneLineaVision(t))
                    {
                        mejor = t; mejorDist = d;
                    }
                }
            }
            objetivo = mejor;
        }

        // Cambiar estado según resultado
        if (objetivo)
        {
            SetEstado(Estado.Perseguir);
        }
        else
        {
            // si estaba persiguiendo y lo perdió, que vuelva
            if (estado == Estado.Perseguir)
                SetEstado(Estado.Volver);
        }
    }

    bool TieneLineaVision(Transform t)
    {
        Vector3 origenRay = transform.position + Vector3.up * 0.6f;
        Vector3 destinoRay = t.position + Vector3.up * 0.6f;

        if (Physics.Linecast(origenRay, destinoRay, out RaycastHit hit, capasObstaculos, QueryTriggerInteraction.Ignore))
        {
            Debug.Log("Bloqueado por: " + hit.collider.name);
            return false;
        }
        Debug.Log("Ve al jugador sin bloqueo");
        return true;
    }


    void BuscarNuevoDestinoMerodeo(bool inmediato)
    {
        tProximoDestino = Time.time + (inmediato ? 0.1f : intervaloNuevoDestino);

        // punto aleatorio alrededor de "origen" dentro del radioMerodeo
        Vector3 random = Random.insideUnitSphere * radioMerodeo;
        random.y = 0f;
        Vector3 candidato = origen + random;

        // proyectar al NavMesh
        if (NavMesh.SamplePosition(candidato, out NavMeshHit hit, 4f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            // si falló, pruebo cerca del lobo
            Vector3 fallback = transform.position + Random.insideUnitSphere * (radioMerodeo * 0.5f);
            fallback.y = 0f;
            if (NavMesh.SamplePosition(fallback, out hit, 4f, NavMesh.AllAreas))
                agent.SetDestination(hit.position);
        }
    }

    void IntentarAtacar()
    {
        if (Time.time < tPuedeAtacar) return;
        tPuedeAtacar = Time.time + cdAtaque;

        // Acá disparás animación/daño. Ejemplo genérico:
        // Si tu jugador tiene un script de vida, lo buscás y le aplicás daño.
        if (objetivo)
        {
            var vida = objetivo.GetComponentInParent<IDañoRecibible>(); // interfaz opcional
            if (vida != null) vida.RecibirDaño(dano);
            // Si no usás interfaz, podés mandar un SendMessage o un evento propio.
        }
    }

    void SetEstado(Estado e)
    {
        if (estado == e) return;
        estado = e;
        // Hook para animaciones si querés:
        // animator.SetFloat("Speed", agent.speed);
        // animator.SetTrigger(e.ToString());
    }

    // ------------------- GIZMOS -------------------
    void OnDrawGizmosSelected()
    {
        if (!dibujarGizmos) return;

        Gizmos.color = new Color(0.2f, 0.7f, 1f, 0.25f);
        Gizmos.DrawWireSphere((Application.isPlaying ? origen : transform.position), radioMerodeo);

        Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);

        if (agent && agent.hasPath)
        {
            Gizmos.color = Color.yellow;
            var path = agent.path;
            for (int i = 1; i < path.corners.Length; i++)
                Gizmos.DrawLine(path.corners[i - 1], path.corners[i]);
        }
    }
}

// Interfaz mínima opcional para recibir daño
public interface IDañoRecibible
{
    void RecibirDaño(int cantidad);
}
*/
using UnityEngine;
using UnityEngine.AI;

public class WolfAI : MonoBehaviour
{
    public enum Estado { Merodear, Perseguir, Volver }

    [Header("Refs")]
    public NavMeshAgent agent;
    [Tooltip("Capas que se consideran objetivos (ej. Player)")]
    public LayerMask capasObjetivo = ~0;
    [Tooltip("Capas que bloquean línea de visión (paredes/rocas)")]
    public LayerMask capasObstaculos;

    [Header("Detección")]
    public float radioDeteccion = 12f;
    public bool usarLineaVision = true;
    public float refrescoDeteccion = 0.2f;

    [Header("Movimiento")]
    public float radioMerodeo = 15f;      // radio alrededor del punto inicial
    public float umbralLlegada = 0.6f;    // distancia para considerar que llegó
    public float intervaloNuevoDestino = 3f;
    public float velocidadMerodeo = 2.2f;
    public float velocidadPersecucion = 4.0f;

    [Header("Persecución")]
    public float distanciaMaxPersecucion = 20f; // límite para abandonar persecución

    [Header("Combate (opcional)")]
    public float rangoAtaque = 1.8f;
    public float cdAtaque = 1.0f;
    public int dano = 10;

    [Header("Debug")]
    public bool dibujarGizmos = true;

    private Estado estado = Estado.Merodear;
    private Vector3 origen;
    private Transform objetivo;
    private float tSiguienteChequeo = 0f;
    private float tProximoDestino = 0f;
    private float tPuedeAtacar = 0f;

    void Reset()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Awake()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        origen = transform.position;
    }

    void OnEnable()
    {
        SetEstado(Estado.Merodear);
        BuscarNuevoDestinoMerodeo(true);
    }

    void Update()
    {
        // 1) detectar objetivos cada cierto tiempo
        if (Time.time >= tSiguienteChequeo)
        {
            tSiguienteChequeo = Time.time + refrescoDeteccion;
            ActualizarDeteccion();
        }

        // 2) FSM simple
        switch (estado)
        {
            case Estado.Merodear:
                agent.speed = velocidadMerodeo;
                if ((!agent.pathPending && agent.remainingDistance <= umbralLlegada)
                    || Time.time >= tProximoDestino)
                {
                    BuscarNuevoDestinoMerodeo(false);
                }
                break;

            case Estado.Perseguir:
                agent.speed = velocidadPersecucion;
                if (objetivo)
                {
                    MoverA(objetivo.position);

                    float dist = Vector3.Distance(transform.position, objetivo.position);

                    // si se aleja demasiado, abandonar persecución
                    if (dist > distanciaMaxPersecucion)
                    {
                        SetEstado(Estado.Volver);
                        objetivo = null;
                        break;
                    }

                    // atacar si está cerca
                    if (dist <= rangoAtaque)
                    {
                        IntentarAtacar();
                        // mantener un poco de espacio
                        Vector3 offset = (transform.position - objetivo.position).normalized * 0.5f;
                        MoverA(objetivo.position + offset);
                    }
                }
                else
                {
                    SetEstado(Estado.Volver);
                }
                break;

            case Estado.Volver:
                agent.speed = velocidadMerodeo;
                MoverA(origen);

                if (!agent.pathPending && agent.remainingDistance <= umbralLlegada)
                {
                    SetEstado(Estado.Merodear);
                    BuscarNuevoDestinoMerodeo(true);
                }
                break;
        }
    }

    // ------------------- LÓGICA -------------------

    void ActualizarDeteccion()
    {
        // buscar el objetivo válido más cercano
        Collider[] hits = Physics.OverlapSphere(transform.position, radioDeteccion, capasObjetivo, QueryTriggerInteraction.Ignore);
        Transform mejor = null; float mejorDist = float.MaxValue;

        foreach (var h in hits)
        {
            Transform t = h.attachedRigidbody ? h.attachedRigidbody.transform : h.transform;
            float d = Vector3.SqrMagnitude(t.position - transform.position);
            if (d < mejorDist)
            {
                if (!usarLineaVision || TieneLineaVision(t))
                {
                    mejor = t; mejorDist = d;
                }
            }
        }
        objetivo = mejor;

        if (objetivo)
            SetEstado(Estado.Perseguir);
        else if (estado == Estado.Perseguir)
            SetEstado(Estado.Volver);
    }

    bool TieneLineaVision(Transform t)
    {
        Vector3 origenRay = transform.position + Vector3.up * 0.6f;
        Vector3 destinoRay = t.position + Vector3.up * 0.6f;

        if (Physics.Linecast(origenRay, destinoRay, out RaycastHit hit, capasObstaculos, QueryTriggerInteraction.Ignore))
        {
            return false; // algo bloquea
        }
        return true;
    }

    void BuscarNuevoDestinoMerodeo(bool inmediato)
    {
        tProximoDestino = Time.time + (inmediato ? 0.1f : intervaloNuevoDestino);
        Vector3 random = Random.insideUnitSphere * radioMerodeo;
        random.y = 0f;
        Vector3 candidato = origen + random;

        if (NavMesh.SamplePosition(candidato, out NavMeshHit hit, 4f, NavMesh.AllAreas))
        {
            MoverA(hit.position);
        }
    }

    void IntentarAtacar()
    {
        if (Time.time < tPuedeAtacar) return;
        tPuedeAtacar = Time.time + cdAtaque;

        if (objetivo)
        {
            var vida = objetivo.GetComponentInParent<IDañoRecibible>();
            if (vida != null) vida.RecibirDaño(dano);
        }
    }

    void MoverA(Vector3 destino)
    {
        if (agent && agent.isActiveAndEnabled && agent.isOnNavMesh)
            agent.SetDestination(destino);
    }

    void SetEstado(Estado e)
    {
        if (estado == e) return;
        estado = e;
    }

    // ------------------- GIZMOS -------------------
    void OnDrawGizmosSelected()
    {
        if (!dibujarGizmos) return;

        Gizmos.color = new Color(0.2f, 0.7f, 1f, 0.25f);
        Gizmos.DrawWireSphere(Application.isPlaying ? origen : transform.position, radioMerodeo);

        Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
    }
}

// Interfaz opcional para recibir daño
public interface IDañoRecibible
{
    void RecibirDaño(int cantidad);
}
