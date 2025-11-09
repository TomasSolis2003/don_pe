/*using UnityEngine;
using System.Collections.Generic;

public class SpawnerEnemigos : MonoBehaviour
{
    [Header("Configuración de Spawn")]
    public GameObject prefabEnemigo;
    public List<Transform> puntosSpawn = new List<Transform>();
    public int maxEnemigosPorNoche = 3;

    private readonly List<GameObject> enemigosActivos = new List<GameObject>();

    void OnEnable()
    {
        SunMovement.OnCambioDiaNoche += OnCambioDiaNoche;
    }

    void OnDisable()
    {
        SunMovement.OnCambioDiaNoche -= OnCambioDiaNoche;
    }

    void OnCambioDiaNoche(bool esDia)
    {
        if (esDia)
        {
            // Día: destruir enemigos activos
            foreach (var e in enemigosActivos)
                if (e != null) Destroy(e);

            enemigosActivos.Clear();
        }
        else
        {
            // Noche: spawnear de 1 a maxEnemigosPorNoche
            int cantidad = Random.Range(1, maxEnemigosPorNoche + 1);
            for (int i = 0; i < cantidad; i++)
            {
                Transform punto = puntosSpawn[Random.Range(0, puntosSpawn.Count)];
                GameObject nuevo = Instantiate(prefabEnemigo, punto.position, Quaternion.identity);
                enemigosActivos.Add(nuevo);
            }
            Debug.Log($"🌒 Aparecieron {cantidad} enemigos en la noche.");
        }
    }
}
*/
using UnityEngine;
using System.Collections.Generic;

public class SpawnerEnemigosArea : MonoBehaviour
{
    [Header("Configuración de Spawn")]
    public GameObject prefabEnemigo;
    public Vector2 tamañoArea = new Vector2(40f, 40f); // ancho y largo
    public int maxEnemigosPorNoche = 3;
    public LayerMask capaTerreno;

    private readonly List<GameObject> enemigosActivos = new List<GameObject>();

    void OnEnable()
    {
        SunMovement.OnCambioDiaNoche += OnCambioDiaNoche;
    }

    void OnDisable()
    {
        SunMovement.OnCambioDiaNoche -= OnCambioDiaNoche;
    }

    void OnCambioDiaNoche(bool esDia)
    {
        if (esDia)
        {
            // Día → limpiar enemigos
            foreach (var e in enemigosActivos)
                if (e != null) Destroy(e);
            enemigosActivos.Clear();
        }
        else
        {
            // Noche → generar entre 1 y maxEnemigosPorNoche
            int cantidad = Random.Range(1, maxEnemigosPorNoche + 1);
            for (int i = 0; i < cantidad; i++)
            {
                Vector3 pos = GenerarPosicionAleatoria();
                GameObject nuevo = Instantiate(prefabEnemigo, pos, Quaternion.identity);
                enemigosActivos.Add(nuevo);
            }
            Debug.Log($"🌒 Aparecieron {cantidad} enemigos en el área nocturna.");
        }
    }

    Vector3 GenerarPosicionAleatoria()
    {
        Vector3 basePos = transform.position;
        Vector3 random = new Vector3(
            Random.Range(-tamañoArea.x / 2, tamañoArea.x / 2),
            100f,
            Random.Range(-tamañoArea.y / 2, tamañoArea.y / 2)
        );
        Vector3 spawnPos = basePos + random;

        // Ajustar a terreno si existe
        if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 200f, capaTerreno))
            spawnPos = hit.point;

        return spawnPos;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawCube(transform.position, new Vector3(tamañoArea.x, 0.2f, tamañoArea.y));
    }
}
