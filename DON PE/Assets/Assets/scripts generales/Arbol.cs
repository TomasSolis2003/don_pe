


/*using UnityEngine;
using System.Collections;

public class Arbol : MonoBehaviour
{
    [Header("Configuración del árbol")]
    public bool esGigante = false;
    public int troncosNormales = 3;
    public int troncosGigantes = 8;
    public int vidaNormal = 3;     // golpes para talar un árbol normal
    public int vidaGigante = 6;    // golpes para talar un árbol gigante

    public GameObject prefabTronco;

    private int vidaActual;
    private bool cortado = false;
    private SueloFertil suelo; // referencia al suelo que lo generó
    private Vector3 posicionOriginal; // guardamos la posición inicial

    void Awake()
    {
        posicionOriginal = transform.position;

        // Si por algún motivo el prefab viene sin inicializar, lo corregimos
        if (name.ToLower().Contains("gigante"))
            esGigante = true;

        if (vidaActual <= 0)
        {
            vidaActual = esGigante ? vidaGigante : vidaNormal;
            Debug.Log($"🌲 [Awake] {name} configurado como {(esGigante ? "GIGANTE" : "Normal")} con {vidaActual} de vida inicial.");
        }
    }

    public void Inicializar(SueloFertil sueloFertil, bool gigante, GameObject troncoPrefab)
    {
        suelo = sueloFertil;
        esGigante = gigante;
        prefabTronco = troncoPrefab;

        // ⚡ Forzamos la asignación correcta de vida según tipo
        vidaActual = esGigante ? vidaGigante : vidaNormal;

        Debug.Log($"🌳 [Inicializar] Árbol {(esGigante ? "GIGANTE" : "Normal")} asignado con {vidaActual} de vida.");
    }

    // llamado desde el jugador
    public void RecibirGolpe(int daño = 1)
    {
        if (cortado) return;

        vidaActual -= daño;
        Debug.Log($"💥 {name} recibió {daño} de daño. Vida restante: {vidaActual}");

        StartCoroutine(MoverArbol());

        if (vidaActual <= 0)
            Cortar();
    }

    IEnumerator MoverArbol()
    {
        Vector3 abajo = posicionOriginal + Vector3.down * 1f;
        float t = 0f;

        while (t < 0.1f)
        {
            transform.position = Vector3.Lerp(posicionOriginal, abajo, t / 0.1f);
            t += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.05f);

        t = 0f;
        while (t < 0.2f)
        {
            transform.position = Vector3.Lerp(abajo, posicionOriginal, t / 0.2f);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = posicionOriginal;
    }

    void Cortar()
    {
        cortado = true;
        int cantidad = esGigante ? troncosGigantes : troncosNormales;

        Debug.Log($"🪓 {name} talado. Generando {cantidad} troncos.");

        for (int i = 0; i < cantidad; i++)
        {
            Vector3 pos = transform.position + new Vector3(
                Random.Range(-1.5f, 1.5f),
                0.5f,
                Random.Range(-1.5f, 1.5f)
            );
            Instantiate(prefabTronco, pos, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}

*/
/*using UnityEngine;

public class Arbol : MonoBehaviour
{
    [Header("Configuración de tala")]
    public int golpesNecesarios = 5;
    private int golpesRecibidos = 0;
    private bool talado = false;

    [Header("Drop de troncos")]
    public GameObject prefabTronco;
    public int cantidadTroncos = 3;
    public SueloFertil sueloFertil;

    [Header("Efecto visual")]
    public float desplazamientoGolpe = 0.1f;
    public float velocidadRebote = 8f;

    private Vector3 posicionOriginal;
    private Vector3 posicionObjetivo;
    private float tiempoRebote = 0f;
    private bool estaRebotando = false;

    void Start()
    {
        posicionOriginal = transform.position;

        if (sueloFertil == null)
            sueloFertil = GetComponentInParent<SueloFertil>();
    }

    void Update()
    {
        if (estaRebotando)
        {
            tiempoRebote += Time.deltaTime * velocidadRebote;
            transform.position = Vector3.Lerp(transform.position, posicionObjetivo, tiempoRebote);

            if (Vector3.Distance(transform.position, posicionObjetivo) < 0.01f)
            {
                if (posicionObjetivo == posicionOriginal)
                {
                    estaRebotando = false;
                }
                else
                {
                    posicionObjetivo = posicionOriginal;
                    tiempoRebote = 0f;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (talado) return;

        if (other.CompareTag("Hacha"))
        {
            RecibirGolpe();
        }
    }
    public void Inicializar(SueloFertil suelo, bool esGigante, GameObject prefab)
    {
        sueloFertil = suelo;
        prefabTronco = prefab;
    }
    public void RecibirGolpe()
    {
        if (talado) return;

        golpesRecibidos++;
        Debug.Log($"Golpe al árbol ({golpesRecibidos}/{golpesNecesarios})");

        // pequeño rebote visual
        posicionObjetivo = posicionOriginal + Vector3.down * desplazamientoGolpe;
        tiempoRebote = 0f;
        estaRebotando = true;

        if (golpesRecibidos >= golpesNecesarios)
        {
            Talado();
        }
    }

    void Talado()
    {
        talado = true;

        // generar troncos
        for (int i = 0; i < cantidadTroncos; i++)
        {
            Vector3 dropPos = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0.3f, Random.Range(-0.5f, 0.5f));
            Instantiate(prefabTronco, dropPos, Quaternion.identity);
        }

        // avisar al terreno
        if (sueloFertil != null)
            sueloFertil.ArbolTalado(transform.position);

        Destroy(gameObject);
    }
}
*/
/*using UnityEngine;

public class Arbol : MonoBehaviour
{
    [Header("Configuración de tala")]
    public int golpesNecesarios = 5;
    private int golpesRecibidos = 0;
    private bool talado = false;

    [Header("Drop de troncos")]
    public GameObject prefabTronco;
    public int cantidadTroncos = 3;
    public SueloFertil sueloFertil;

    [Header("Efecto visual")]
    public float desplazamientoGolpe = 0.1f;
    public float velocidadRebote = 8f;

    private Vector3 posicionOriginal;
    private Vector3 posicionObjetivo;
    private float tiempoRebote = 0f;
    private bool estaRebotando = false;

    void Start()
    {
        posicionOriginal = transform.position;

        if (sueloFertil == null)
            sueloFertil = GetComponentInParent<SueloFertil>();
    }

    void Update()
    {
        if (estaRebotando)
        {
            tiempoRebote += Time.deltaTime * velocidadRebote;
            transform.position = Vector3.Lerp(transform.position, posicionObjetivo, tiempoRebote);

            if (Vector3.Distance(transform.position, posicionObjetivo) < 0.01f)
            {
                if (posicionObjetivo == posicionOriginal)
                {
                    estaRebotando = false;
                }
                else
                {
                    posicionObjetivo = posicionOriginal;
                    tiempoRebote = 0f;
                }
            }
        }
    }

    // ✅ Este método lo llama el raycast del hacha
    public void RecibirGolpe()
    {
        if (talado) return;

        golpesRecibidos++;
        Debug.Log($"Golpe al árbol ({golpesRecibidos}/{golpesNecesarios})");

        // pequeña animación visual
        posicionObjetivo = posicionOriginal + Vector3.down * desplazamientoGolpe;
        tiempoRebote = 0f;
        estaRebotando = true;

        if (golpesRecibidos >= golpesNecesarios)
        {
            Talado();
        }
    }

    void Talado()
    {
        talado = true;

        // generar troncos
        for (int i = 0; i < cantidadTroncos; i++)
        {
            Vector3 dropPos = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0.3f, Random.Range(-0.5f, 0.5f));
            Instantiate(prefabTronco, dropPos, Quaternion.identity);
        }

        // avisar al terreno
        if (sueloFertil != null)
            sueloFertil.ArbolTalado(transform.position);

        Destroy(gameObject);
    }
}
*/
using UnityEngine;

public class Arbol : MonoBehaviour
{
    [Header("Configuración de tala")]
    public int golpesNecesarios = 5;
    private int golpesRecibidos = 0;
    private bool talado = false;

    [Header("Drop de troncos")]
    public GameObject prefabTronco;
    public int cantidadTroncos = 3;
    public SueloFertil sueloFertil;

    [Header("Efecto visual")]
    public float desplazamientoGolpe = 0.4f; // más visible
    public float velocidadRebote = 2f;

    private Vector3 posicionOriginal;
    private Vector3 posicionObjetivo;
    private float tiempoRebote = 0f;
    private bool estaRebotando = false;

    void Start()
    {
        posicionOriginal = transform.position;

        if (sueloFertil == null)
            sueloFertil = GetComponentInParent<SueloFertil>();
    }

    void Update()
    {
        if (estaRebotando)
        {
            tiempoRebote += Time.deltaTime * velocidadRebote;
            transform.position = Vector3.Lerp(transform.position, posicionObjetivo, tiempoRebote);

            if (Vector3.Distance(transform.position, posicionObjetivo) < 0.01f)
            {
                if (posicionObjetivo == posicionOriginal)
                {
                    estaRebotando = false;
                }
                else
                {
                    posicionObjetivo = posicionOriginal;
                    tiempoRebote = 0f;
                }
            }
        }
    }

    // ✅ Llamado por el raycast del hacha
    public void RecibirGolpe()
    {
        if (talado) return;

        golpesRecibidos++;
        Debug.Log($"🌲 Golpe al árbol ({golpesRecibidos}/{golpesNecesarios})");

        // animación visible
        posicionObjetivo = posicionOriginal + Vector3.down * desplazamientoGolpe;
        tiempoRebote = 0f;
        estaRebotando = true;

        if (golpesRecibidos >= golpesNecesarios)
        {
            Debug.Log("🌳 Árbol talado!");
            StartCoroutine(CaerYDestruir());
        }
    }

    private System.Collections.IEnumerator CaerYDestruir()
    {
        talado = true;

        // Pequeña animación de caída
        float duracion = 1.2f;
        float tiempo = 0f;
        Quaternion rotInicial = transform.rotation;
        Quaternion rotFinal = Quaternion.Euler(rotInicial.eulerAngles + new Vector3(0f, 0f, Random.Range(60f, 90f)));

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(rotInicial, rotFinal, tiempo / duracion);
            yield return null;
        }

        // generar troncos
        for (int i = 0; i < cantidadTroncos; i++)
        {
            Vector3 dropPos = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0.3f, Random.Range(-0.5f, 0.5f));
            Instantiate(prefabTronco, dropPos, Quaternion.identity);
        }

        // avisar al suelo fértil
        if (sueloFertil != null)
            sueloFertil.ArbolTalado(transform.position);

        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
