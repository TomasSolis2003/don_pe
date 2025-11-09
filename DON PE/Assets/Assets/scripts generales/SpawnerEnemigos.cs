
/*using UnityEngine;
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
*/
/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerEnemigosArea : MonoBehaviour
{
    [Header("Configuración de Spawn")]
    public GameObject prefabEnemigo;
    public Vector2 tamañoArea = new Vector2(40f, 40f);
    [Range(1, 20)] public int maxEnemigosPorNoche = 8;
    [Range(0f, 2f)] public float multiplicadorFrecuencia = 1.5f; // 1 = normal, >1 = más enemigos
    public LayerMask capaTerreno;

    [Header("Spawn escalonado")]
    public float tiempoEntreSpawns = 0.5f; // segundos entre cada enemigo

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
            // 🌙 Noche → spawn más agresivo
            StartCoroutine(SpawnNocturnoCR());
        }
    }

    IEnumerator SpawnNocturnoCR()
    {
        int baseCantidad = Random.Range(2, maxEnemigosPorNoche + 1);
        int cantidadFinal = Mathf.RoundToInt(baseCantidad * multiplicadorFrecuencia);
        cantidadFinal = Mathf.Clamp(cantidadFinal, 1, maxEnemigosPorNoche);

        Debug.Log($"🌒 Aparecerán {cantidadFinal} duendes esta noche...");

        for (int i = 0; i < cantidadFinal; i++)
        {
            Vector3 pos = GenerarPosicionAleatoria();
            GameObject nuevo = Instantiate(prefabEnemigo, pos, Quaternion.identity);
            enemigosActivos.Add(nuevo);

            // Espera antes de generar el siguiente
            yield return new WaitForSeconds(tiempoEntreSpawns);
        }

        Debug.Log($"✨ Se generaron {cantidadFinal} enemigos en el área {name}");
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

        // Ajustar al terreno
        if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 200f, capaTerreno))
            spawnPos = hit.point;

        return spawnPos;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.9f, 0.2f, 0.2f, 0.3f);
        Gizmos.DrawCube(transform.position, new Vector3(tamañoArea.x, 0.2f, tamañoArea.y));
    }
}
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerEnemigosArea : MonoBehaviour
{
    [Header("Configuración de Spawn")]
    public GameObject prefabEnemigo;
    public Vector2 tamañoArea = new Vector2(40f, 40f);
    [Range(1, 20)] public int maxEnemigosPorNoche = 8;
    [Range(0f, 2f)] public float multiplicadorFrecuencia = 1.5f;
    public LayerMask capaTerreno;

    [Header("Referencias globales")]
    public Transform jugador;
    public GameObject prefabTierraSinPreparar;
    public Transform[] viasDeEscape;

    [Header("Spawn escalonado")]
    public float tiempoEntreSpawns = 0.5f;

    private readonly List<GameObject> enemigosActivos = new List<GameObject>();

    void OnEnable() => SunMovement.OnCambioDiaNoche += OnCambioDiaNoche;
    void OnDisable() => SunMovement.OnCambioDiaNoche -= OnCambioDiaNoche;

    void OnCambioDiaNoche(bool esDia)
    {
        if (esDia)
        {
            foreach (var e in enemigosActivos)
                if (e != null) Destroy(e);
            enemigosActivos.Clear();
        }
        else
        {
            StartCoroutine(SpawnNocturnoCR());
        }
    }

    IEnumerator SpawnNocturnoCR()
    {
        int baseCantidad = Random.Range(2, maxEnemigosPorNoche + 1);
        int cantidadFinal = Mathf.RoundToInt(baseCantidad * multiplicadorFrecuencia);
        cantidadFinal = Mathf.Clamp(cantidadFinal, 1, maxEnemigosPorNoche);

        Debug.Log($"🌒 Aparecerán {cantidadFinal} duendes esta noche...");

        for (int i = 0; i < cantidadFinal; i++)
        {
            Vector3 pos = GenerarPosicionAleatoria();
            GameObject nuevo = Instantiate(prefabEnemigo, pos, Quaternion.identity);
            enemigosActivos.Add(nuevo);

            // --- 🔥 Auto-asignación directa ---
            var ia = nuevo.GetComponent<EnemyIA_Duende>();
            if (ia != null)
            {
                if (jugador == null && GameObject.FindGameObjectWithTag("Player"))
                    jugador = GameObject.FindGameObjectWithTag("Player").transform;

                ia.jugadorManual = jugador;

                if (prefabTierraSinPreparar == null)
                    prefabTierraSinPreparar = GameObject.Find("TierraSinPrepararPrefab");

                ia.prefabTierraSinPreparar = prefabTierraSinPreparar;

                if (viasDeEscape == null || viasDeEscape.Length == 0)
                {
                    GameObject[] escapes = GameObject.FindGameObjectsWithTag("Escape");
                    ia.viasDeEscape = new Transform[escapes.Length];
                    for (int j = 0; j < escapes.Length; j++)
                        ia.viasDeEscape[j] = escapes[j].transform;
                }
                else
                {
                    ia.viasDeEscape = viasDeEscape;
                }
            }

            yield return new WaitForSeconds(tiempoEntreSpawns);
        }

        Debug.Log($"✨ Se generaron {cantidadFinal} enemigos en el área {name}");
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

        if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 200f, capaTerreno))
            spawnPos = hit.point;

        return spawnPos;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.9f, 0.2f, 0.2f, 0.3f);
        Gizmos.DrawCube(transform.position, new Vector3(tamañoArea.x, 0.2f, tamañoArea.y));
    }
}
