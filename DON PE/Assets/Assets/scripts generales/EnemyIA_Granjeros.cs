

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIA_Granjeros : MonoBehaviour, IDañoRecibible
{
    private enum Estado { Buscando, YendoObjetivo, Comiendo, PersiguiendoJugador, Huyendo }
    // --- NUEVO CAMPO ---
    [Header("Destrucción de cultivos")]
    public GameObject prefabTierraSinPreparar; // prefab de tierra vacía (sin sembrar)

    [Header("Percepción")]
    public float radioDeteccion = 15f;
    public float intervaloBusqueda = 0.5f;
    public LayerMask capasDetectables;
    public string tagTrigo = "trigo";
    public string tagHoguera = "Hoguera";
    public string tagLobo = "LoboSalvaje";

    [Header("Jugador")]
    public Transform jugadorManual;
    public LayerMask capasJugador;
    public float radioDeteccionJugador = 12f;
    public int danoAlJugador = 10;
    public float rangoAtaqueJugador = 1.7f;
    public float cdAtaqueJugador = 1.2f;

    [Header("NavMesh")]
    public NavMeshAgent agent;
    public float velocidadNormal = 3.5f;
    public float velocidadHuida = 9f;

    [Header("Acciones")]
    public float distanciaAccion = 1.5f;
    public int danoATrigo = 10;

    [Header("Salud")]
    public int vidaMaxima = 20;
    private int vidaActual;

    [Header("Patrulla")]
    public float radioMerodeo = 10f;
    public float tiempoEsperaPatrulla = 3f;

    [Header("Huida")]
    public float duracionHuida = 15f;
    public Transform[] viasDeEscape;
    public float distanciaLlegadaEscape = 1.5f;
    public AudioClip sonidoHuida;
    public ParticleSystem particulasHuida;

    [Header("Debug")]
    public bool dibujarGizmos = true;

    private Estado estado = Estado.Buscando;
    private GameObject objetivoActual;
    private Transform objetivoJugador;
    private Vector3 puntoOrigen;
    private float timerPatrulla = 0f;
    private float proximaBusqueda = 0f;
    private bool puedeAtacar = true;
    private Coroutine comerCR;
    private bool estaVivo = true;
    private AudioSource audioSrc;

    void Start()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        puntoOrigen = transform.position;
        agent.speed = velocidadNormal;
        vidaActual = vidaMaxima;
        audioSrc = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!estaVivo) return;

        if (Time.time >= proximaBusqueda && (estado == Estado.Buscando || estado == Estado.YendoObjetivo))
        {
            proximaBusqueda = Time.time + intervaloBusqueda;
            BuscarNuevoObjetivo();
        }

        switch (estado)
        {
            case Estado.Buscando:
                Patrullar();
                break;

            case Estado.YendoObjetivo:
                if (objetivoActual == null)
                {
                    estado = Estado.Buscando;
                    break;
                }
                MoverA(objetivoActual.transform.position);

                float dist = Vector3.Distance(transform.position, objetivoActual.transform.position);
                if (dist <= distanciaAccion)
                    EjecutarAccionSobreObjetivo();
                break;

            case Estado.Comiendo:
                break;

            case Estado.PersiguiendoJugador:
                if (objetivoJugador == null)
                {
                    estado = Estado.Buscando;
                    break;
                }
                MoverA(objetivoJugador.position);

                float dj = Vector3.Distance(transform.position, objetivoJugador.position);
                if (dj <= rangoAtaqueJugador && puedeAtacar)
                    StartCoroutine(AtaqueJugadorCR());
                break;

            case Estado.Huyendo:
                // manejado por corrutina
                break;
        }
    }

    // --------------------------- BUSCAR OBJETIVOS ---------------------------
    void BuscarNuevoObjetivo()
    {
        GameObject hoguera = BuscarMasCercanoPorTag(tagHoguera);
        if (hoguera != null) { objetivoActual = hoguera; estado = Estado.YendoObjetivo; return; }

        GameObject lobo = BuscarMasCercanoPorTag(tagLobo);
        if (lobo != null) { objetivoActual = lobo; estado = Estado.YendoObjetivo; return; }

        GameObject trigo = BuscarMasCercanoPorTag(tagTrigo);
        if (trigo != null) { objetivoActual = trigo; estado = Estado.YendoObjetivo; return; }

        objetivoActual = null;
        estado = Estado.Buscando;
    }

    GameObject BuscarMasCercanoPorTag(string tag)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
        GameObject masCercano = null;
        float distMin = Mathf.Infinity;
        foreach (var o in objs)
        {
            float d = Vector3.Distance(transform.position, o.transform.position);
            if (d < distMin && d <= radioDeteccion)
            {
                distMin = d;
                masCercano = o;
            }
        }
        return masCercano;
    }

    void EjecutarAccionSobreObjetivo()
    {
        if (objetivoActual == null)
        {
            estado = Estado.Buscando;
            return;
        }

        if (objetivoActual.CompareTag(tagHoguera))
        {
            Destroy(objetivoActual);
            Morir();
        }
        else if (objetivoActual.CompareTag(tagLobo))
        {
            var vidaLobo = objetivoActual.GetComponent<IDañoRecibible>();
            if (vidaLobo != null) vidaLobo.RecibirDaño(vidaMaxima);
            objetivoActual = null;
            estado = Estado.Buscando;
        }
        else if (objetivoActual.CompareTag(tagTrigo))
        {
            if (comerCR == null) comerCR = StartCoroutine(ComerTrigoCR(objetivoActual));
        }
        else
        {
            objetivoActual = null;
            estado = Estado.Buscando;
        }
    }

    /* IEnumerator ComerTrigoCR(GameObject trigoGO)
     {
         estado = Estado.Comiendo;
         agent.ResetPath();

         var vidaTrigo = trigoGO ? trigoGO.GetComponent<IDañoRecibible>() : null;
         if (vidaTrigo != null) vidaTrigo.RecibirDaño(danoATrigo);

         yield return new WaitForSeconds(1f);

         comerCR = null;
         objetivoActual = null;
         estado = Estado.Buscando;


     }
  */
    IEnumerator ComerTrigoCR(GameObject trigoGO)
    {
        estado = Estado.Comiendo;
        agent.ResetPath();

        if (!trigoGO) { comerCR = null; estado = Estado.Buscando; yield break; }

        // Asegurar marcador
        var marcador = trigoGO.GetComponent<TrigoConsumible>();
        if (!marcador) marcador = trigoGO.AddComponent<TrigoConsumible>();

        // Si ya fue procesado, salir
        if (marcador.procesado) { comerCR = null; estado = Estado.Buscando; yield break; }

        // Marcar antes de hacer nada (esto evita duplicados aunque entren múltiples llamadas)
        marcador.procesado = true;

        // Posición base
        Vector3 pos = trigoGO.transform.position;
        pos.y = 0.95f; // tu altura

        // (Opcional) Adaptar al terreno
        if (Physics.Raycast(pos + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 5f))
            pos = hit.point;

        // Instanciar tierra una sola vez
        if (prefabTierraSinPreparar != null)
            Instantiate(prefabTierraSinPreparar, pos, Quaternion.identity);
        else
            Debug.LogWarning($"{name}: Falta prefabTierraSinPreparar");

        // Dañar o destruir el trigo
        var vidaTrigo = trigoGO.GetComponent<IDañoRecibible>();
        if (vidaTrigo != null) vidaTrigo.RecibirDaño(danoATrigo);
        else Destroy(trigoGO);

        // pequeña pausa “comer”
        yield return new WaitForSeconds(1f);

        comerCR = null;
        objetivoActual = null;
        estado = Estado.Buscando;
    }
    
 

    // --------------------------- ATAQUE JUGADOR ---------------------------
    IEnumerator AtaqueJugadorCR()
    {
        puedeAtacar = false;
        var vida = objetivoJugador ? objetivoJugador.GetComponent<IDañoRecibible>() : null;
        if (vida != null)
            vida.RecibirDaño(danoAlJugador);

        yield return new WaitForSeconds(cdAtaqueJugador);
        puedeAtacar = true;
    }

    // --------------------------- DETECCIÓN JUGADOR ---------------------------
    void DetectarJugador()
    {
        if (jugadorManual != null)
        {
            objetivoJugador = jugadorManual;
            estado = Estado.PersiguiendoJugador;
            agent.speed = velocidadNormal * 1.2f;
            return;
        }

        Collider[] found = Physics.OverlapSphere(transform.position, radioDeteccionJugador, capasJugador, QueryTriggerInteraction.Ignore);
        if (found.Length > 0)
        {
            Transform candidato = found[0].attachedRigidbody ? found[0].attachedRigidbody.transform : found[0].transform;
            objetivoJugador = candidato.root;
            estado = Estado.PersiguiendoJugador;
            agent.speed = velocidadNormal * 1.2f;
        }
    }

    // --------------------------- RECIBIR DAÑO ---------------------------
    public void RecibirDaño(int cantidad)
    {
        if (!estaVivo) return;

        vidaActual -= cantidad;
        Debug.Log($"{name} recibió {cantidad} de daño. Vida restante: {vidaActual}");

        if (vidaActual <= 0)
        {
            Morir();
            return;
        }

        // Al perder la mitad de la vida, decide atacar o huir
        if (vidaActual <= vidaMaxima / 2)
        {
            if (Random.value < 0.5f)
                DetectarJugador();
            else
                IniciarHuida();
        }
        else if (Random.value < 0.25f)
            IniciarHuida();
    }

    // --------------------------- HUIDA CON VÍAS FIJAS ---------------------------
    void IniciarHuida()
    {
        if (viasDeEscape == null || viasDeEscape.Length == 0)
        {
            Debug.LogWarning($"{name} no tiene vías de escape asignadas.");
            return;
        }

        estado = Estado.Huyendo;
        objetivoActual = null;
        objetivoJugador = null;

        // sonido y partículas opcionales
        if (sonidoHuida && audioSrc) audioSrc.PlayOneShot(sonidoHuida);
        if (particulasHuida) particulasHuida.Play();

        // elegir una de las vías
        Transform destino = viasDeEscape[Random.Range(0, viasDeEscape.Length)];
        agent.speed = velocidadHuida;
        agent.SetDestination(destino.position);

        StartCoroutine(MonitorearHuida(destino));
    }

    IEnumerator MonitorearHuida(Transform destino)
    {
        float tiempo = 0f;

        while (estado == Estado.Huyendo && tiempo < duracionHuida)
        {
            if (destino == null) break;

            float dist = Vector3.Distance(transform.position, destino.position);
            if (dist <= distanciaLlegadaEscape)
            {
                Desaparecer();
                yield break;
            }

            tiempo += Time.deltaTime;
            yield return null;
        }

        Desaparecer();
    }

    void Desaparecer()
    {
        if (!estaVivo) return;
        estaVivo = false;
        agent.isStopped = true;
        Destroy(gameObject, 0.1f);
    }

    void Morir()
    {
        if (!estaVivo) return;
        estaVivo = false;
        Destroy(gameObject);
    }

    // --------------------------- MOVIMIENTO AUX ---------------------------
    void MoverA(Vector3 destino)
    {
        if (agent && agent.isActiveAndEnabled && agent.isOnNavMesh)
            agent.SetDestination(destino);
    }

    void Patrullar()
    {
        timerPatrulla += Time.deltaTime;
        if (timerPatrulla >= tiempoEsperaPatrulla)
        {
            Vector3 randomDir = Random.insideUnitSphere * radioMerodeo; randomDir.y = 0;
            Vector3 candidato = puntoOrigen + randomDir;

            if (NavMesh.SamplePosition(candidato, out NavMeshHit hit, radioMerodeo, NavMesh.AllAreas))
                MoverA(hit.position);

            timerPatrulla = 0f;
        }
    }

    // --------------------------- GIZMOS ---------------------------
    void OnDrawGizmosSelected()
    {
        if (!dibujarGizmos) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeteccionJugador);
        Gizmos.color = Color.cyan;
        if (viasDeEscape != null)
        {
            foreach (var v in viasDeEscape)
                if (v != null) Gizmos.DrawSphere(v.position, 0.4f);
        }
    }
}



