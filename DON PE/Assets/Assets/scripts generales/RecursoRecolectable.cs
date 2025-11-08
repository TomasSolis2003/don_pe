using UnityEngine;

public class RecursoRecolectable : MonoBehaviour
{
    [Header("Configuración")]
    public string tipoRecurso = "Piedra";  // puede ser "Tronco", "Piedra", etc.
    public int cantidad = 1;
    public float distanciaRecogida = 2f;

    [Header("Desaparición automática")]
    public float tiempoVida = 20f; // se destruye si no lo levantás

    private Transform jugador;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (tiempoVida > 0)
            Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector3.Distance(transform.position, jugador.position);
        if (distancia <= distanciaRecogida && Input.GetKeyDown(KeyCode.E))
        {
            Recoger();
        }
    }

    void Recoger()
    {
        InventarioJugador inv = jugador.GetComponent<InventarioJugador>();
        if (inv != null)
        {
       //     inv.AgregarRecurso(tipoRecurso, cantidad);
            Debug.Log($"Recogiste {cantidad} {tipoRecurso}");
        }
        Destroy(gameObject);
    }
}
