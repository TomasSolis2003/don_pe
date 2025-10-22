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

    private bool enProgreso = false;

    void Start()
    {
        if (!esFinal && siguienteEtapaPrefab != null)
            StartCoroutine(CicloCrecimiento());
    }

    System.Collections.IEnumerator CicloCrecimiento()
    {
        enProgreso = true;
        yield return new WaitForSeconds(tiempoParaSiguiente);

        if (fxCambio) Instantiate(fxCambio, transform.position, Quaternion.identity);
        if (sfxCambio) sfxCambio.Play();

        // Instanciar siguiente etapa en el mismo lugar y rotación
        Instantiate(siguienteEtapaPrefab, transform.position, transform.rotation);

        // Destruir este prefab actual
        Destroy(gameObject);
    }
}
