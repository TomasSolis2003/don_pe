/*using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sunLight; // Luz del sol
    public float dayDuration = 120f; // Duración de un día completo en segundos
    public Color dayColor = Color.white; // Color de la luz de día
    public Color nightColor = Color.blue; // Color de la luz de noche
    public float moonIntensity = 0.3f; // Intensidad de la luz lunar

    private float timeOfDay = 0f; // Tiempo actual del día (0 a 1)
    private float sunInitialIntensity; // Intensidad inicial del sol

    private void Start()
    {
        // Guardar la intensidad inicial de la luz del sol
        if (sunLight != null)
        {
            sunInitialIntensity = sunLight.intensity;
        }
    }

    private void Update()
    {
        // Incrementar el tiempo del día (de 0 a 1)
        timeOfDay += Time.deltaTime / dayDuration;

        if (timeOfDay > 1f) // Si el día terminó, reiniciar
        {
            timeOfDay = 0f;
        }

        // Ajustar la rotación del sol para simular el ciclo de día/noche
        if (sunLight != null)
        {
            sunLight.transform.rotation = Quaternion.Euler((timeOfDay * 360f) - 90f, 0f, 0f);

            // Cambiar la intensidad y color de la luz según el ciclo de día y noche
            sunLight.color = Color.Lerp(nightColor, dayColor, timeOfDay);
            sunLight.intensity = Mathf.Lerp(moonIntensity, sunInitialIntensity, timeOfDay);

            // Si quieres que la luz de la luna sea visible en la noche, ajusta la intensidad
            if (timeOfDay >= 0.5f) // Durante la noche
            {
                sunLight.intensity = moonIntensity;
            }
        }
    }
}
*/
/*using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Ciclo de Día y Noche")]
    public Light sunLight; // Luz del sol
    public float dayDuration = 120f; // Duración de un día completo en segundos
    public Color dayColor = Color.white; // Color de la luz de día
    public Color nightColor = Color.blue; // Color de la luz de noche
    public float moonIntensity = 0.3f; // Intensidad de la luz lunar

    [Header("Diablo Configuración")]
    public GameObject diabloPrefab; // Prefab del enemigo "Diablo"
    public Transform spawnPoint; // Punto donde se spawnea el "Diablo"
    public float nightThreshold = 0.5f; // Umbral para considerar que es de noche
    public float boxColliderInterval = 5f; // Intervalo entre cada generación de BoxCollider

    private float timeOfDay = 0f; // Tiempo actual del día (0 a 1)
    private float sunInitialIntensity; // Intensidad inicial del sol
    private GameObject diabloInstance; // Referencia al "Diablo" spawneado
    private bool isNight = false; // Estado de si es de noche

    private void Start()
    {
        // Guardar la intensidad inicial de la luz del sol
        if (sunLight != null)
        {
            sunInitialIntensity = sunLight.intensity;
        }
    }

    private void Update()
    {
        // Incrementar el tiempo del día (de 0 a 1)
        timeOfDay += Time.deltaTime / dayDuration;

        if (timeOfDay > 1f) // Si el día terminó, reiniciar
        {
            timeOfDay = 0f;
        }

        // Ajustar la rotación del sol para simular el ciclo de día/noche
        if (sunLight != null)
        {
            sunLight.transform.rotation = Quaternion.Euler((timeOfDay * 360f) - 90f, 0f, 0f);

            // Cambiar la intensidad y color de la luz según el ciclo de día y noche
            sunLight.color = Color.Lerp(nightColor, dayColor, timeOfDay);
            sunLight.intensity = Mathf.Lerp(moonIntensity, sunInitialIntensity, timeOfDay);

            if (timeOfDay >= nightThreshold && !isNight)
            {
                // Activar el estado de noche
                isNight = true;
                SpawnDiablo();
            }
            else if (timeOfDay < nightThreshold && isNight)
            {
                // Desactivar el estado de noche
                isNight = false;
                DespawnDiablo();
            }
        }
    }

    private void SpawnDiablo()
    {
        if (diabloPrefab != null && spawnPoint != null)
        {
            diabloInstance = Instantiate(diabloPrefab, spawnPoint.position, Quaternion.identity);
            diabloInstance.AddComponent<DiabloBehavior>().Initialize(boxColliderInterval);
        }
        else
        {
            Debug.LogWarning("Faltan referencias para spawnear al Diablo.");
        }
    }

    private void DespawnDiablo()
    {
        if (diabloInstance != null)
        {
            Destroy(diabloInstance);
        }
    }
}
*/
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo
    public Transform[] spawnPoints; // Lugares donde pueden aparecer los enemigos
    public float dayDuration = 30f; // Duración del día en segundos
    public float nightDuration = 30f; // Duración de la noche en segundos
    public int maxEnemies = 5; // Máximo de enemigos en escena

    private bool isNight = false;
    private float cycleTimer = 0f;

    void Update()
    {
        cycleTimer += Time.deltaTime;

        if (!isNight && cycleTimer >= dayDuration) // Cambia a noche
        {
            isNight = true;
            cycleTimer = 0f;
            Debug.Log("🌙 Comienza la noche: ¡Los enemigos pueden aparecer!");
        }
        else if (isNight && cycleTimer >= nightDuration) // Cambia a día
        {
            isNight = false;
            cycleTimer = 0f;
            EliminarEnemigos();
            Debug.Log("☀️ Comienza el día: No hay enemigos.");
        }

        if (isNight)
        {
            SpawnEnemigos();
        }
    }

    void SpawnEnemigos()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    void EliminarEnemigos()
    {
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemigo in enemigos)
        {
            Destroy(enemigo);
        }
    }
}
