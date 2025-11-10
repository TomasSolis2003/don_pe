using UnityEngine;
using TMPro;

public class Reloj : MonoBehaviour
{
    [Header("Entrada")]
    public SunMovement hora;
    [Range(0f, 24f)]
    public float horaActual = 12f;  // valor que puede venir desde otro script o texto (0–24)

    [Header("Referencias (opcional)")]
    public TextMeshProUGUI textoEstado;   // Texto para mostrar "Día", "Tarde", "Noche"
    public TextMeshProUGUI textoHora;     // Texto para mostrar la hora en formato AM/PM
    public TextMeshProUGUI textoDia;      // Texto para mostrar el número de día

    public enum EstadoDia { Dia, Tarde, Noche }
    public EstadoDia estadoActual;

    public int numeroDia = 1;
    private float ultimaHora = 0f;

    void Start()
    {
        // Mostrar el día inicial apenas empieza el juego
        if (textoDia != null)
            textoDia.text = $"Día {numeroDia}";
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

        if (horaActual >= 6f && horaActual < 12f)
            estadoActual = EstadoDia.Dia;
        else if (horaActual >= 12f && horaActual < 18f)
            estadoActual = EstadoDia.Tarde;
        else
            estadoActual = EstadoDia.Noche;

        if (textoEstado != null)
            textoEstado.text = estadoActual.ToString();
    }

    // --- Muestra la hora formateada ---
    void ActualizarHora()
    {
        if (textoHora == null) return;

        float hora12 = horaActual % 12f;
        if (hora12 == 0f) hora12 = 12f;  // para que no diga 0 AM o 0 PM

        string sufijo = (horaActual < 12f) ? "AM" : "PM";
        int horaRedondeada = Mathf.FloorToInt(hora12);
        int minutos = Mathf.FloorToInt((horaActual - Mathf.Floor(horaActual)) * 60);

        textoHora.text = $"{horaRedondeada:00}:{minutos:00} {sufijo}";
    }

    // --- Detecta cuando pasa de 24h y suma un día ---
    void VerificarCambioDeDia()
    {
        if (horaActual < ultimaHora) // cuando pasa de 23.99 a 0
        {
            numeroDia++;
            if (textoDia != null)
                textoDia.text = $"Día {numeroDia}";
        }
        ultimaHora = horaActual;
    }

    // --- Permite setear la hora desde otro texto ---
    public void SetHoraDesdeTexto(string valorTexto)
    {
        if (float.TryParse(valorTexto, out float hora))
        {
            horaActual = Mathf.Clamp(hora, 0f, 24f);
            ActualizarEstado();
        }
        else
        {
            Debug.LogWarning("Valor de hora inválido: " + valorTexto);
        }
    }
}
