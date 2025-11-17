
/*using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Comida : MonoBehaviour
{
    [Header("Efectos sobre el jugador")]
    [Tooltip("Cantidad de vida que cura o daña. Positivo = cura, Negativo = daño.")]
    public int vidaRestaurada = 0;

    [Tooltip("Cuánto llena la barra de hambre.")]
    public int saciedad = 20;

    [Tooltip("¿Se destruye el objeto después de comerlo?")]
    public bool destruirAlComer = true;

    [Header("Interacción")]
    public float distanciaInteraccion = 2f;
    public KeyCode teclaComer = KeyCode.E;

    private bool jugadorCerca = false;
    private PlayerHealth jugador;

    void Start()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;
    }

    void Update()
    {
        if (jugadorCerca && jugador != null && Input.GetKeyDown(teclaComer))
        {
            Consumir();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugador = other.GetComponent<PlayerHealth>();
            jugadorCerca = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugador = null;
            jugadorCerca = false;
        }
    }

    public void Consumir()
    {
        if (jugador == null) return;

        // Efecto sobre la vida
        if (vidaRestaurada > 0)
            jugador.Curar(vidaRestaurada);
        else if (vidaRestaurada < 0)
            jugador.RecibirDaño(Mathf.Abs(vidaRestaurada));

        // Efecto sobre el hambre
        PlayerHambre hambre = jugador.GetComponent<PlayerHambre>();
        if (hambre != null)
            hambre.CambiarHambre(saciedad);

        Debug.Log($"Jugador comió {name} → Hambre +{saciedad}, Vida {vidaRestaurada}");

        if (destruirAlComer)
            Destroy(gameObject);
    }
}
*/
/*using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Comida : MonoBehaviour
{
    [Header("Efectos sobre el jugador")]
    [Tooltip("Cantidad de vida que cura o daña. Positivo = cura, Negativo = daño.")]
    public int vidaRestaurada = 0;

    [Tooltip("Cuánto llena la barra de hambre.")]
    public int saciedad = 20;

    [Tooltip("¿Se destruye el objeto después de comerlo?")]
    public bool destruirAlComer = true;

    [Header("Interacción")]
    public float distanciaInteraccion = 2f;
    public KeyCode teclaComer = KeyCode.E;

    // --- NUEVO: referencias opcionales para respawn ---
    private ArbustoMoras arbustoOrigen;
    private Transform puntoOrigen;

    private bool jugadorCerca = false;
    private PlayerHealth jugador;

    void Start()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;
    }

    void Update()
    {
        if (jugadorCerca && jugador != null && Input.GetKeyDown(teclaComer))
        {
            Consumir();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugador = other.GetComponent<PlayerHealth>();
            jugadorCerca = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugador = null;
            jugadorCerca = false;
        }
    }

    public void Consumir()
    {
        if (jugador == null) return;

        // Efecto sobre la vida
        if (vidaRestaurada > 0)
            jugador.Curar(vidaRestaurada);
        else if (vidaRestaurada < 0)
            jugador.RecibirDaño(Mathf.Abs(vidaRestaurada));

        // Efecto sobre el hambre
        PlayerHambre hambre = jugador.GetComponent<PlayerHambre>();
        if (hambre != null)
            hambre.CambiarHambre(saciedad);

        Debug.Log($"Jugador comió {name} → Hambre +{saciedad}, Vida {vidaRestaurada}");

        // --- NUEVO: notificar respawn si viene de un arbusto ---
        if (arbustoOrigen != null)
            arbustoOrigen.NotificarMoraRecolectada(gameObject, puntoOrigen);

        if (destruirAlComer)
            Destroy(gameObject);
    }

    // --- NUEVO: para que el arbusto asigne origen ---
    public void AsignarOrigen(ArbustoMoras arbusto, Transform punto)
    {
        arbustoOrigen = arbusto;
        puntoOrigen = punto;
    }
}
*/
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Comida : MonoBehaviour
{
    [Header("Efectos sobre el jugador")]
    [Tooltip("Cantidad de vida que cura o daña. Positivo = cura, Negativo = daño.")]
    public int vidaRestaurada = 0;

    [Tooltip("Cuánto llena la barra de hambre.")]
    public int saciedad = 20;

    [Tooltip("¿Se destruye el objeto después de comerlo?")]
    public bool destruirAlComer = true;

    [Header("Interacción")]
    public float distanciaInteraccion = 2f;
    public KeyCode teclaComer = KeyCode.E;

    // --- NUEVO BLOQUE DE AUDIO ---
    [Header("Sonido al comer")]
    public AudioClip sonidoComer;
    public AudioSource fuenteAudio;
    // Si no asignás nada, se creará uno automático.

    // --- NUEVO: referencias opcionales para respawn ---
    private ArbustoMoras arbustoOrigen;
    private Transform puntoOrigen;

    private bool jugadorCerca = false;
    private PlayerHealth jugador;

    void Start()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;

        // Crear AudioSource automáticamente si el usuario no asignó uno
        if (fuenteAudio == null)
        {
            fuenteAudio = gameObject.AddComponent<AudioSource>();
            fuenteAudio.playOnAwake = false;
        }
    }

    void Update()
    {
        if (jugadorCerca && jugador != null && Input.GetKeyDown(teclaComer))
        {
            Consumir();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugador = other.GetComponent<PlayerHealth>();
            jugadorCerca = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugador = null;
            jugadorCerca = false;
        }
    }

    public void Consumir()
    {
        if (jugador == null) return;

        // --- SONIDO AL COMER ---
        if (sonidoComer != null && fuenteAudio != null)
            fuenteAudio.PlayOneShot(sonidoComer);

        // Efecto sobre la vida
        if (vidaRestaurada > 0)
            jugador.Curar(vidaRestaurada);
        else if (vidaRestaurada < 0)
            jugador.RecibirDaño(Mathf.Abs(vidaRestaurada));

        // Efecto sobre el hambre
        PlayerHambre hambre = jugador.GetComponent<PlayerHambre>();
        if (hambre != null)
            hambre.CambiarHambre(saciedad);

        Debug.Log($"Jugador comió {name} → Hambre +{saciedad}, Vida {vidaRestaurada}");

        // --- Avisar al arbusto (respawn) ---
        if (arbustoOrigen != null)
            arbustoOrigen.NotificarMoraRecolectada(gameObject, puntoOrigen);

        // ⚠️ Importante: destruir *después* del sonido.
        // Dejar que el sonido suene sin cortar.
        if (destruirAlComer)
            Destroy(gameObject, sonidoComer != null ? sonidoComer.length : 0f);
    }

    // --- NUEVO: asignar origen para respawn ---
    public void AsignarOrigen(ArbustoMoras arbusto, Transform punto)
    {
        arbustoOrigen = arbusto;
        puntoOrigen = punto;
    }
}
