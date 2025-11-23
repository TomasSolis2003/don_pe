using UnityEngine;
using System.Collections.Generic;

public class SpawnerConejos : MonoBehaviour
{
    [Header("Configuración del Spawner")]
    public GameObject prefabConejo;
    public float radioSpawn = 10f;
    public float tiempoEntreSpawns = 5f;
    public int maxConejos = 8;

    [Header("Altura del spawn")]
    public float offsetY = 0.5f;

    private float temporizador = 0f;
    private List<GameObject> conejosVivos = new List<GameObject>();

    void Update()
    {
        // Limpiar nulos (cuando un conejo muere)
        conejosVivos.RemoveAll(c => c == null);

        // Si ya hay demasiados, no spawnear
        if (conejosVivos.Count >= maxConejos)
            return;

        temporizador += Time.deltaTime;

        if (temporizador >= tiempoEntreSpawns)
        {
            SpawnConejo();
            temporizador = 0f;
        }
    }

    void SpawnConejo()
    {
        Vector3 punto = ObtenerPuntoAleatorio();

        GameObject nuevoConejo = Instantiate(prefabConejo, punto, Quaternion.identity);
        conejosVivos.Add(nuevoConejo);
    }

    Vector3 ObtenerPuntoAleatorio()
    {
        Vector2 random = Random.insideUnitCircle * radioSpawn;
        Vector3 pos = new Vector3(
            transform.position.x + random.x,
            transform.position.y + offsetY,
            transform.position.z + random.y
        );

        return pos;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.25f);
        Gizmos.DrawSphere(transform.position, radioSpawn);
    }
}
