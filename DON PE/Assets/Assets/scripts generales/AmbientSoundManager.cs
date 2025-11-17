/*using UnityEngine;

public class AmbientSoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource diaSource;      // Ambiente de día
    public AudioSource nocheSource;    // Ambiente de noche
    public AudioSource vientoSource;   // Viento constante

    [Header("Crossfade")]
    public float fadeSpeed = 1f;

    private bool esDeDia = true;

    void OnEnable()
    {
        // Suscribirse al evento del sol
        SunMovement.OnCambioDiaNoche += CambiarAmbiente;
    }

    void OnDisable()
    {
        // Desuscribirse para evitar errores
        SunMovement.OnCambioDiaNoche -= CambiarAmbiente;
    }

    void Start()
    {
        // Forzar loop
        if (diaSource != null) diaSource.loop = true;
        if (nocheSource != null) nocheSource.loop = true;
        if (vientoSource != null) vientoSource.loop = true;

        // Reproducir todos los loops
        if (diaSource != null) diaSource.Play();
        if (nocheSource != null) nocheSource.Play();
        if (vientoSource != null) vientoSource.Play();

        // Configurar estado inicial
        CambiarAmbiente(esDeDia);
    }

    void Update()
    {
        // Hacer crossfade suave
        float diaTarget = esDeDia ? 1f : 0f;
        float nocheTarget = esDeDia ? 0f : 1f;

        if (diaSource != null)
            diaSource.volume = Mathf.MoveTowards(diaSource.volume, diaTarget, fadeSpeed * Time.deltaTime);

        if (nocheSource != null)
            nocheSource.volume = Mathf.MoveTowards(nocheSource.volume, nocheTarget, fadeSpeed * Time.deltaTime);

        // El viento se mantiene constante (o editás su volumen si querés)
    }

    // Recibe el evento desde SunMovement
    private void CambiarAmbiente(bool esDia)
    {
        esDeDia = esDia;
        Debug.Log(esDeDia ? "🎵 Ambiente: Día" : "🎵 Ambiente: Noche");
    }
}
*/
using UnityEngine;
using System.Collections;

public class AmbientSoundManager : MonoBehaviour
{
    [Header("Audio Sources (loops)")]
    public AudioSource nocheSource;    // Ambiente nocturno (loop)
    public AudioSource vientoSource;   // Viento constante (loop)

    [Header("Sonidos aleatorios de día")]
    public AudioSource diaRandomSource; // AudioSource para reproducir sonidos sueltos
    public AudioClip[] sonidosDia;      // Lista de sonidos (incluí tu coyote aquí)
    public float minDelayDia = 5f;      // Mínimo entre sonidos
    public float maxDelayDia = 20f;     // Máximo entre sonidos

    [Header("Crossfade")]
    public float fadeSpeed = 1f;

    private bool esDeDia = true;
    private Coroutine rutinaDia;

    void OnEnable()
    {
        SunMovement.OnCambioDiaNoche += CambiarAmbiente;
    }

    void OnDisable()
    {
        SunMovement.OnCambioDiaNoche -= CambiarAmbiente;
    }

    void Start()
    {
        // Viento siempre en loop
        if (vientoSource != null)
        {
            vientoSource.loop = true;
            vientoSource.Play();
        }

        // Noche en loop
        if (nocheSource != null)
        {
            nocheSource.loop = true;
            nocheSource.volume = 0f;
            nocheSource.Play();
        }

        // Arrancar estado según atributo inicial
        CambiarAmbiente(esDeDia);
    }

    void Update()
    {
        // Fades
        float nocheTarget = esDeDia ? 0f : 1f;

        if (nocheSource != null)
            nocheSource.volume = Mathf.MoveTowards(nocheSource.volume, nocheTarget, fadeSpeed * Time.deltaTime);
    }

    private void CambiarAmbiente(bool esDia)
    {
        esDeDia = esDia;

        if (esDeDia)
        {
            // Iniciar sonidos aleatorios del día
            if (rutinaDia != null)
                StopCoroutine(rutinaDia);

            rutinaDia = StartCoroutine(ReproducirSonidosAleatoriosDia());
        }
        else
        {
            // Detener sonidos aleatorios de día
            if (rutinaDia != null)
                StopCoroutine(rutinaDia);
        }
    }

    IEnumerator ReproducirSonidosAleatoriosDia()
    {
        while (esDeDia)
        {
            // Esperar un intervalo aleatorio
            float delay = Random.Range(minDelayDia, maxDelayDia);
            yield return new WaitForSeconds(delay);

            // Reproducir sonido aleatorio
            if (sonidosDia.Length > 0 && diaRandomSource != null)
            {
                AudioClip clip = sonidosDia[Random.Range(0, sonidosDia.Length)];
                diaRandomSource.PlayOneShot(clip);
            }
        }
    }
}
