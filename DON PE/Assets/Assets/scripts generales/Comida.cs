/*using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Comida : MonoBehaviour
{
    [Header("Efectos sobre el jugador")]
    [Tooltip("Cantidad de vida que cura o daña. Positivo = cura, Negativo = daño.")]
    public int vidaRestaurada = 10;

    [Tooltip("¿Se destruye el objeto después de comerlo?")]
    public bool destruirAlComer = true;

    [Header("Interacción")]
    [Tooltip("Distancia máxima a la que el jugador puede comer esto.")]
    public float distanciaInteraccion = 2f;

    [Tooltip("Tecla para comer manualmente (si el jugador está cerca).")]
    public KeyCode teclaComer = KeyCode.E;

    private bool jugadorCerca = false;
    private PlayerHealth jugador;

    void Start()
    {
        // Asegurar que el collider esté en modo trigger
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
            Debug.Log($"El jugador puede comer {name}");
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

        // Afectar salud si es curativo o dañino
        if (vidaRestaurada != 0)
        {
            if (vidaRestaurada > 0)
                jugador.Curar(vidaRestaurada);
            else
                jugador.RecibirDaño(Mathf.Abs(vidaRestaurada));
        }

        // Afectar hambre si el jugador tiene el sistema de hambre
        PlayerHambre hambre = jugador.GetComponent<PlayerHambre>();
        if (hambre != null)
            hambre.CambiarHambre(+20); // cantidad que llena (podés hacerlo variable por comida)

        Debug.Log($"Jugador comió {name}, +20 hambre");

        if (destruirAlComer)
            Destroy(gameObject);
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
