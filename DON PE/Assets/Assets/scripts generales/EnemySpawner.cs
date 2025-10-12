using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemigoPrefab;
    public Transform[] puntosSpawn; // Lugares donde pueden aparecer
    public int cantidadPorNoche = 3;
    public float tiempoEntreSpawns = 5f;

    [Header("Referencia al sol / ciclo")]
    public SunMovement sol;

    private bool nocheActiva = false;
    private float timerSpawn = 0f;
    private int spawnsHechos = 0;

    void Update()
    {
        if (sol == null) return;

        // Detectar si estamos de noche
        bool esNoche = !sol.esDeDia;

        // Al entrar a la noche reiniciamos contador
        if (esNoche && !nocheActiva)
        {
            nocheActiva = true;
            spawnsHechos = 0;
            timerSpawn = 0f;
        }

        // Al volver al día se apaga el spawner
        if (!esNoche && nocheActiva)
        {
            nocheActiva = false;
        }

        // Si estamos en noche, ir spawneando
        if (nocheActiva && spawnsHechos < cantidadPorNoche)
        {
            timerSpawn += Time.deltaTime;
            if (timerSpawn >= tiempoEntreSpawns)
            {
                timerSpawn = 0f;
                SpawnEnemigo();
            }
        }
    }

    void SpawnEnemigo()
    {
        if (puntosSpawn.Length == 0 || enemigoPrefab == null) return;

        Transform punto = puntosSpawn[Random.Range(0, puntosSpawn.Length)];
        Instantiate(enemigoPrefab, punto.position, punto.rotation);
        spawnsHechos++;
    }
}
