using UnityEngine;
using TMPro;
using System.Collections;

public class Reloj : MonoBehaviour
{
    [Header("Entrada")]
    public SunMovement hora;
    [Range(0f, 24f)]
    public float horaActual = 12f;

    [Header("Referencias (opcional)")]
    public TextMeshProUGUI textoEstado;   // "Amanecer", "Día", "Atardecer", "Noche"
    public TextMeshProUGUI textoHora;     // "10:30 AM"
    public TextMeshProUGUI textoDia;      // "Día 1"

    [Header("Spawner de Horda")]
    public SpawnerHorda Horda;

    public enum EstadoDia { Amanecer, Dia, Atardecer, Noche }
    public EstadoDia estadoActual;
    private EstadoDia ultimoEstado;

    public int numeroDia = 1;
    private float ultimaHora = 0f;
    private Coroutine mostrarTextoCoroutine;

    void Start()
    {
        if (textoDia != null)
            textoDia.text = $"Día {numeroDia}";

        if (textoEstado != null)
            textoEstado.gameObject.SetActive(false);
    }

    void Update()
    {
        ActualizarEstado();
        ActualizarHora();
        VerificarCambioDeDia();
    }

    // --- Clasificación del momento del día ---
    public void ActualizarEstado()
    {
        horaActual = hora.currentHour;

        if (horaActual >= 5f && horaActual < 8f)
            estadoActual = EstadoDia.Amanecer;
        else if (horaActual >= 8f && horaActual < 17f)
            estadoActual = EstadoDia.Dia;
        else if (horaActual >= 17f && horaActual < 20f)
            estadoActual = EstadoDia.Atardecer;
        else
            estadoActual = EstadoDia.Noche;

        if (estadoActual != ultimoEstado)
        {
            MostrarTextoConFade(estadoActual.ToString(), 3f);
            ultimoEstado = estadoActual;
        }
    }

    // --- Mostrar texto con fade in/out ---
    void MostrarTextoConFade(string texto, float duracionVisible)
    {
        if (textoEstado == null) return;

        if (mostrarTextoCoroutine != null)
            StopCoroutine(mostrarTextoCoroutine);

        mostrarTextoCoroutine = StartCoroutine(FadeTextoRutina(texto, duracionVisible));
    }

    IEnumerator FadeTextoRutina(string texto, float duracionVisible)
    {
        textoEstado.gameObject.SetActive(true);
        textoEstado.text = texto;

        // Fade IN
        Color color = textoEstado.color;
        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(0f, 1f, t);
            textoEstado.color = color;
            yield return null;
        }

        color.a = 1f;
        textoEstado.color = color;

        // Esperar visible
        yield return new WaitForSeconds(duracionVisible);

        // Fade OUT
        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(1f, 0f, t);
            textoEstado.color = color;
            yield return null;
        }

        color.a = 0f;
        textoEstado.color = color;
        textoEstado.gameObject.SetActive(false);
    }

    // --- Mostrar hora ---
    void ActualizarHora()
    {
        if (textoHora == null) return;

        float hora12 = horaActual % 12f;
        if (hora12 == 0f) hora12 = 12f;

        string sufijo = (horaActual < 12f) ? "AM" : "PM";
        int horaRedondeada = Mathf.FloorToInt(hora12);
        int minutos = Mathf.FloorToInt((horaActual - Mathf.Floor(horaActual)) * 60);

        textoHora.text = $"{horaRedondeada:00}:{minutos:00} {sufijo}";
    }

    // --- Cambio de día (solo se ejecuta una vez cada 24h) ---
    void VerificarCambioDeDia()
    {
        // Detecta cuando pasa de 23.99 -> 0
        if (horaActual < ultimaHora)
        {
            numeroDia++;

            if (textoDia != null)
                textoDia.text = $"Día {numeroDia}";

            // 🧩 SOLO AQUÍ se ejecuta la verificación (una vez por día)
            if (Horda != null)
            {
                if (numeroDia % 5 == 0)
                {
                    Horda.spawnear = true;
                    Debug.Log($"✅ Horda activada en el Día {numeroDia}");
                }
                else
                {
                    Horda.spawnear = false;
                    Debug.Log($"🕓 Día {numeroDia}, sin horda");
                }
            }
        }

        ultimaHora = horaActual;
    }
}
