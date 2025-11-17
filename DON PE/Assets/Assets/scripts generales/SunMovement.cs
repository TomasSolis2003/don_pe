
using UnityEngine;

public class SunMovement : MonoBehaviour
{
    [Header("Duración del ciclo")]
    [Tooltip("Duración total del día en minutos reales")]
    public float dayLengthInMinutes = 10f;


    [Header("Horario inicial")]
    [Range(0, 24)]
    public float startHour = 8f; // 8 AM

    [Header("Definición de día y noche")]
    [Range(0, 24)] public float sunriseHour = 6f;
    [Range(0, 24)] public float sunsetHour = 18f;

    [Header("Luz del Sol")]
    [Tooltip("Referencia a la luz direccional que actúa como sol")]
    public Light sol;

    [Tooltip("Color de la luz según la hora del día (0 = medianoche, 1 = siguiente medianoche)")]
    public Gradient colorLuzDia;

    [Tooltip("Curva que define la intensidad del sol a lo largo del día")]
    public AnimationCurve intensidadDia;

    [Header("Estado (debug)")]
    public float currentHour;   // hora actual (0-24)
    public bool esDeDia;

    private float timeSpeed; // horas que pasan por segundo
    private bool estadoAnterior; // para detectar cambios día/noche

    // Evento global para otros scripts
    public delegate void CambioDiaNoche(bool esDia);
    public static event CambioDiaNoche OnCambioDiaNoche;

    void Start()
    {
        // Calcular velocidad del tiempo (cuántas horas pasan por segundo real)
        timeSpeed = 24f / (dayLengthInMinutes * 60f);
        currentHour = startHour;

        // Buscar la luz si no está asignada
        if (sol == null)
            sol = GetComponent<Light>();

        // Inicializar estado
        estadoAnterior = currentHour >= sunriseHour && currentHour < sunsetHour;
        esDeDia = estadoAnterior;
    }

    void Update()
    {
        // Avanzar el reloj
        currentHour += timeSpeed * Time.deltaTime;
        if (currentHour >= 24f)
            currentHour -= 24f;

        // Rotar el sol (0h = medianoche, 6h = amanecer, 12h = mediodía)
        float sunAngle = (currentHour / 24f) * 360f - 90f;
        transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        // Determinar si es de día o de noche
        esDeDia = currentHour >= sunriseHour && currentHour < sunsetHour;

        // Aplicar color e intensidad gradualmente
        ActualizarLuz();

        // Detectar cambio de estado y notificar
        if (esDeDia != estadoAnterior)
        {
            estadoAnterior = esDeDia;
            OnCambioDiaNoche?.Invoke(esDeDia);
            Debug.Log(esDeDia ? "🌞 Amanecer detectado" : "🌙 Anochecer detectado");
        }
    }

    private void ActualizarLuz()
    {
        if (sol == null) return;

        // Normalizar hora entre 0 y 1
        float t = currentHour / 24f;

        // Actualizar color e intensidad desde los gradientes y curvas
        sol.color = colorLuzDia.Evaluate(t);
        sol.intensity = intensidadDia.Evaluate(t);
    }
}
