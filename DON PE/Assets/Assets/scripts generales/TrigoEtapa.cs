
/*using UnityEngine;

public class TrigoEtapa : MonoBehaviour
{
    [Header("Configuración de etapa")]
    public GameObject siguienteEtapaPrefab;  // Qué prefab instanciar después de esta fase
    public float tiempoParaSiguiente = 10f;  // Cuánto tarda en avanzar
    public bool esFinal = false;             // Si es la etapa madura (ya no crece más)

    [Header("Efectos (opcional)")]
    public ParticleSystem fxCambio;
    public AudioSource sfxCambio;

    private bool enProgreso = false;

    void Start()
    {
        // Evita iniciar dos veces el ciclo si ya está en progreso
        if (!esFinal && siguienteEtapaPrefab != null && !enProgreso)
        {
            enProgreso = true;
            StartCoroutine(CicloCrecimiento());
        }
    }

    System.Collections.IEnumerator CicloCrecimiento()
    {
        Debug.Log($"{name} comenzó a crecer..."); // <-- Usamos la variable como estado

        yield return new WaitForSeconds(tiempoParaSiguiente);

        if (fxCambio)
            Instantiate(fxCambio, transform.position, Quaternion.identity);
        if (sfxCambio)
            sfxCambio.Play();

        // Instanciar siguiente etapa en el mismo lugar y rotación
        Instantiate(siguienteEtapaPrefab, transform.position, transform.rotation);

        Debug.Log($"{name} completó su crecimiento."); // <-- Confirmación visual
        enProgreso = false;

        // Destruir este prefab actual
        Destroy(gameObject);
    }
}*/

using UnityEngine;

public class TrigoEtapa : MonoBehaviour
{
    [Header("Configuración de etapa")]
    public GameObject siguienteEtapaPrefab;  // Qué prefab instanciar después de esta fase
    public float tiempoParaSiguiente = 10f;  // Cuánto tarda en avanzar
    public bool esFinal = false;             // Si es la etapa madura (ya no crece más)

    [Header("Efectos (opcional)")]
    public ParticleSystem fxCambio;
    public AudioSource sfxCambio;

    [Header("Variación visual (rotación)")]
    [Tooltip("Variación aleatoria de rotación en el eje Y (en grados).")]
    [Range(0f, 30f)] public float variacionRotacionY = 10f;

    private bool enProgreso = false;

    void Start()
    {
        // Aplicar una rotación aleatoria inicial
        AplicarVariacionRotacion();

        // Si no es la última etapa, comienza el crecimiento
        if (!esFinal && siguienteEtapaPrefab != null && !enProgreso)
        {
            enProgreso = true;
            StartCoroutine(CicloCrecimiento());
        }
    }

    System.Collections.IEnumerator CicloCrecimiento()
    {
        yield return new WaitForSeconds(tiempoParaSiguiente);

        if (fxCambio)
            Instantiate(fxCambio, transform.position, Quaternion.identity);
        if (sfxCambio)
            sfxCambio.Play();

        // Crear la siguiente etapa del trigo
        GameObject nuevaEtapa = Instantiate(siguienteEtapaPrefab, transform.position, transform.rotation);

        // Aplicar una rotación distinta a la nueva etapa
        var script = nuevaEtapa.GetComponent<TrigoEtapa>();
        if (script != null)
            script.AplicarVariacionRotacion();

        Destroy(gameObject);
    }

    public void AplicarVariacionRotacion()
    {
        float rotY = Random.Range(-variacionRotacionY, variacionRotacionY);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + rotY, 0f);
    }
}
