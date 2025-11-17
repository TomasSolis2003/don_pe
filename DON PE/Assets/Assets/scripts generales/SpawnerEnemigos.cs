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

    /* IEnumerator SpawnNocturnoCR()
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
    */
    IEnumerator SpawnNocturnoCR()
    {
        Debug.Log("🌘 Inicio de la noche: spawns constantes activados.");

        // Conseguimos referencia al sol
        SunMovement sun = FindAnyObjectByType<SunMovement>();
        if (sun == null)
        {
            Debug.LogError("❌ No se encontró SunMovement en la escena.");
            yield break;
        }

        int spawnsHechos = 0;
        float tiempo = 0f;
        float intervalo = 6f; // GENERA UN ENEMIGO CADA 6 SEGUNDOS (cambiable)

        // 🔥 BUCLE ACTIVO TODA LA NOCHE
        while (!sun.esDeDia)
        {
            tiempo += Time.deltaTime;

            if (spawnsHechos < maxEnemigosPorNoche && tiempo >= intervalo)
            {
                tiempo = 0f;
                spawnsHechos++;

                Vector3 pos = GenerarPosicionAleatoria();
                GameObject nuevo = Instantiate(prefabEnemigo, pos, Quaternion.identity);
                enemigosActivos.Add(nuevo);

                // Auto asignaciones
                var ia = nuevo.GetComponent<EnemyIA_Duende>();
                if (ia != null)
                {
                    if (jugador == null)
                    {
                        GameObject pl = GameObject.FindGameObjectWithTag("Player");
                        if (pl) jugador = pl.transform;
                    }

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

                Debug.Log($"👹 Spawn #{spawnsHechos}/{maxEnemigosPorNoche} en {name}");
            }

            yield return null;
        }

        Debug.Log("🌅 Amaneció: fin de los spawns nocturnos.");
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
