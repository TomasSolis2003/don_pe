using UnityEngine;

public class Arbol : MonoBehaviour
{
    [Header("Configuración de tala")]
    public int golpesNecesarios = 5;
    private int golpesRecibidos = 0;
    private bool talado = false;
    [Header("Sonido")]
    public AudioSource sonidoGolpe;
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
    /* public void RecibirGolpe()
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
     */
    public void RecibirGolpe()
    {
        if (talado) return;

        // sonido al golpe
        if (sonidoGolpe != null)
            sonidoGolpe.Play();

        golpesRecibidos++;

        posicionObjetivo = posicionOriginal + Vector3.down * desplazamientoGolpe;
        tiempoRebote = 0f;
        estaRebotando = true;

        if (golpesRecibidos >= golpesNecesarios)
        {
            StartCoroutine(CaerYDestruir());
        }
    }
    private System.Collections.IEnumerator CaerYDestruir()
    {
        talado = true;

        // 🔥 DESACTIVAR COLLIDERS DEL ÁRBOL
        Collider[] cols = GetComponentsInChildren<Collider>();
        foreach (var c in cols)
            c.enabled = false;  // o c.isTrigger = true;

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
    public float desplazamientoGolpe = 0.4f;
    public float velocidadRebote = 2f;

    [Header("Sonido")]
    public AudioSource sonidoGolpe;

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

    // Llamado por el hacha
    public void RecibirGolpe()
    {
        if (talado) return;

        // 🔊 reproducir sonido del golpe
        if (sonidoGolpe != null)
            sonidoGolpe.Play();

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

        // Desactivar colliders
        Collider[] cols = GetComponentsInChildren<Collider>();
        foreach (var c in cols)
            c.enabled = false;

        // animación de caída
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
*/