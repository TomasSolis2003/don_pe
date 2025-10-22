/*using UnityEngine;

public class TrigoCreciente : MonoBehaviour
{
    public enum Etapa { Semilla, Brote, MedioMaduro, Maduro }

    [Header("Visuales por etapa (uno u otro esquema)")]
    [Tooltip("Si ya tienes hijos en este objeto, arrástralos aquí.")]
    [SerializeField] private GameObject semillaVisual;
    [SerializeField] private GameObject broteVisual;
    [SerializeField] private GameObject medioMaduroVisual;
    [SerializeField] private GameObject maduroVisual;

    [Header("Duraciones (segundos)")]
    [SerializeField] private float tSemilla_a_Brote = 10f;
    [SerializeField] private float tBrote_a_Medio = 10f;     // relativo desde Brote
    [SerializeField] private float tMedio_a_Maduro = 15f;    // relativo desde Medio

    [Header("Efectos (opcional)")]
    [SerializeField] private ParticleSystem fxCambioEtapa;
    [SerializeField] private AudioSource sfxCambioEtapa;

    [Header("Estado")]
    [SerializeField] private Etapa etapaActual = Etapa.Semilla;

    void Start()
    {
        // Si no asignaste manualmente, intenta buscar por nombre en hijos (opcional)
        AutoWireChildrenIfMissing();

        // Asegura estado visual inicial coherente
        ActualizarVisuales();

        // Arranca el ciclo
        StartCoroutine(Ciclo());
    }

    System.Collections.IEnumerator Ciclo()
    {
        // Semilla -> Brote
        yield return new WaitForSeconds(tSemilla_a_Brote);
        CambiarEtapa(Etapa.Brote);

        // Brote -> Medio maduro
        yield return new WaitForSeconds(tBrote_a_Medio);
        CambiarEtapa(Etapa.MedioMaduro);

        // Medio maduro -> Maduro
        yield return new WaitForSeconds(tMedio_a_Maduro);
        CambiarEtapa(Etapa.Maduro);

        // Aquí queda maduro hasta que lo coseches
    }

    void CambiarEtapa(Etapa nueva)
    {
        etapaActual = nueva;
        ActualizarVisuales();

        if (fxCambioEtapa) Instantiate(fxCambioEtapa, transform.position, Quaternion.identity);
        if (sfxCambioEtapa) sfxCambioEtapa.Play();
    }

    void ActualizarVisuales()
    {
        if (semillaVisual) semillaVisual.SetActive(etapaActual == Etapa.Semilla);
        if (broteVisual) broteVisual.SetActive(etapaActual == Etapa.Brote);
        if (medioMaduroVisual) medioMaduroVisual.SetActive(etapaActual == Etapa.MedioMaduro);
        if (maduroVisual) maduroVisual.SetActive(etapaActual == Etapa.Maduro);
    }

    // Opcional: auto-asigna por nombres si no arrastras referencias
    void AutoWireChildrenIfMissing()
    {
        if (semillaVisual == null) semillaVisual = BuscarHijoPorNombreParcial("Semilla");
        if (broteVisual == null) broteVisual = BuscarHijoPorNombreParcial("Brote");
        if (medioMaduroVisual == null) medioMaduroVisual = BuscarHijoPorNombreParcial("Medio");
        if (maduroVisual == null) maduroVisual = BuscarHijoPorNombreParcial("Maduro");
    }

    GameObject BuscarHijoPorNombreParcial(string contiene)
    {
        foreach (Transform t in transform)
            if (t.name.ToLower().Contains(contiene.ToLower()))
                return t.gameObject;
        return null;
    }
}
*/