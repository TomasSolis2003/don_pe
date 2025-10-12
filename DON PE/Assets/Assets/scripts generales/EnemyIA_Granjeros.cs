/*using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class EnemyIA_Granjeros : MonoBehaviour
{
    [Header("Percepción")]
    public float radioDeteccion = 15f;
    public LayerMask capasDetectables; // terreno donde buscar objetos
    public string tagTrigo = "Trigo";
    public string tagHoguera = "Hoguera";
    public string tagLobo = "Lobo";

    [Header("NavMesh")]
    public NavMeshAgent agent;

    [Header("Acciones")]
    public float distanciaAccion = 1.5f; // qué tan cerca para interactuar
    public int danoATrigo = 10;
    public int vida = 50;

    [Header("Patrulla")]
    public float radioMerodeo = 10f;
    public float tiempoEsperaPatrulla = 3f;

    private GameObject objetivoActual;
    private float timerPatrulla = 0f;
    private Vector3 puntoOrigen;

    void Start()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        puntoOrigen = transform.position;
        BuscarNuevoObjetivo();
    }

    void Update()
    {
        // Si no tenemos objetivo → buscar
        if (objetivoActual == null)
        {
            BuscarNuevoObjetivo();
            return;
        }

        float dist = Vector3.Distance(transform.position, objetivoActual.transform.position);

        if (dist > distanciaAccion)
        {
            agent.SetDestination(objetivoActual.transform.position);
        }
        else
        {
            // Acciones según tipo de objetivo
            if (objetivoActual.CompareTag(tagHoguera))
            {
                // Al tocar la hoguera, se apaga y este muere
                Destroy(objetivoActual);
                Morir();
            }
            else if (objetivoActual.CompareTag(tagLobo))
            {
                // Ataca lobo
                var vidaLobo = objetivoActual.GetComponent<IVida>();
                if (vidaLobo != null) vidaLobo.RecibirDano(vida);
                BuscarNuevoObjetivo();
            }
            else if (objetivoActual.CompareTag(tagTrigo))
            {
                var vidaTrigo = objetivoActual.GetComponent<IVida>();
                if (vidaTrigo != null) vidaTrigo.RecibirDano(danoATrigo);
                // seguir atacando o destruir si ya no tiene vida
            }
        }

        // Si el objetivo murió o se destruyó
        if (objetivoActual == null)
            BuscarNuevoObjetivo();
    }

    void BuscarNuevoObjetivo()
    {
        GameObject hoguera = BuscarMasCercano(tagHoguera);
        if (hoguera != null)
        {
            objetivoActual = hoguera;
            return;
        }

        GameObject lobo = BuscarMasCercano(tagLobo);
        if (lobo != null)
        {
            objetivoActual = lobo;
            return;
        }

        GameObject trigo = BuscarMasCercano(tagTrigo);
        if (trigo != null)
        {
            objetivoActual = trigo;
            return;
        }

        // Si nada encontró → merodear
        Patrullar();
    }

    GameObject BuscarMasCercano(string tag)
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

    void Patrullar()
    {
        timerPatrulla += Time.deltaTime;
        if (timerPatrulla >= tiempoEsperaPatrulla)
        {
            Vector3 randomDir = Random.insideUnitSphere * radioMerodeo;
            randomDir += puntoOrigen;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDir, out hit, radioMerodeo, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
            timerPatrulla = 0f;
        }
    }

    public void RecibirDano(int cantidad)
    {
        vida -= cantidad;
        if (vida <= 0) Morir();
    }

    void Morir()
    {
        Destroy(gameObject);
    }
}
*/
using UnityEngine;
using UnityEngine.AI;

public class EnemyIA_Granjeros : MonoBehaviour
{
    [Header("Percepción")]
    public float radioDeteccion = 15f;
    public LayerMask capasDetectables; // capas donde buscar objetivos
    public string tagTrigo = "Trigo";
    public string tagHoguera = "Hoguera";
    public string tagLobo = "LoboSalvaje"; // <- ataca solo a lobos salvajes

    [Header("NavMesh")]
    public NavMeshAgent agent;

    [Header("Acciones")]
    public float distanciaAccion = 1.5f;
    public int danoATrigo = 10;
    public int vida = 50;

    [Header("Patrulla")]
    public float radioMerodeo = 10f;
    public float tiempoEsperaPatrulla = 3f;

    private GameObject objetivoActual;
    private float timerPatrulla = 0f;
    private Vector3 puntoOrigen;

    void Start()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        puntoOrigen = transform.position;
        BuscarNuevoObjetivo();
    }

    void Update()
    {
        if (objetivoActual == null)
        {
            BuscarNuevoObjetivo();
            return;
        }

        float dist = Vector3.Distance(transform.position, objetivoActual.transform.position);

        if (dist > distanciaAccion)
        {
            agent.SetDestination(objetivoActual.transform.position);
        }
        else
        {
            // --- Acciones según objetivo ---
            if (objetivoActual.CompareTag(tagHoguera))
            {
                Destroy(objetivoActual);
                Morir(); // se sacrifica al apagar la hoguera
            }
            else if (objetivoActual.CompareTag(tagLobo))
            {
                var vidaLobo = objetivoActual.GetComponent<IDañoRecibible>();
                if (vidaLobo != null) vidaLobo.RecibirDaño(vida);
                BuscarNuevoObjetivo();
            }
            else if (objetivoActual.CompareTag(tagTrigo))
            {
                var vidaTrigo = objetivoActual.GetComponent<IDañoRecibible>();
                if (vidaTrigo != null) vidaTrigo.RecibirDaño(danoATrigo);
                // sigue atacando mientras exista
            }
        }

        if (objetivoActual == null)
            BuscarNuevoObjetivo();
    }

    void BuscarNuevoObjetivo()
    {
        GameObject hoguera = BuscarMasCercano(tagHoguera);
        if (hoguera != null)
        {
            objetivoActual = hoguera;
            return;
        }

        GameObject lobo = BuscarMasCercano(tagLobo);
        if (lobo != null)
        {
            objetivoActual = lobo;
            return;
        }

        GameObject trigo = BuscarMasCercano(tagTrigo);
        if (trigo != null)
        {
            objetivoActual = trigo;
            return;
        }

        Patrullar();
    }

    GameObject BuscarMasCercano(string tag)
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

    void Patrullar()
    {
        timerPatrulla += Time.deltaTime;
        if (timerPatrulla >= tiempoEsperaPatrulla)
        {
            Vector3 randomDir = Random.insideUnitSphere * radioMerodeo;
            randomDir += puntoOrigen;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDir, out hit, radioMerodeo, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
            timerPatrulla = 0f;
        }
    }

    public void RecibirDaño(int cantidad)
    {
        vida -= cantidad;
        if (vida <= 0) Morir();
    }

    void Morir()
    {
        Destroy(gameObject);
    }
}
