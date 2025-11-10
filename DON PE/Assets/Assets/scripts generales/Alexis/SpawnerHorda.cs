using UnityEngine;

public class SpawnerHorda : MonoBehaviour
{
    [Header("Configuración del Spawn")]
    public GameObject prefabEnemigo;     // Prefab del enemigo a instanciar
    public int cantidad = 5;             // Cuántos enemigos spawnear
    public float radioSpawn = 20f;       // Radio máximo del spawn
    public float distanciaMin = 10f;     // Mínima distancia del jugador

    [Header("Spawner")]
    public Transform Spawner;            // Referencia al jugador (asignar en inspector)

    [Header("Altura del terreno (opcional)")]
    public float altura = 0f;            // Si el mapa es plano, usar 0

    void Start()
    {
        if (!Spawner)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) Spawner = p.transform;
        }

        SpawnEnemigos();
    }

    public void SpawnEnemigos()
    {
        if (prefabEnemigo == null || Spawner == null) return;

        for (int i = 0; i < cantidad; i++)
        {
            Vector3 pos = GenerarPosicionLejana();
            Instantiate(prefabEnemigo, pos, Quaternion.identity);
        }
    }

    Vector3 GenerarPosicionLejana()
    {
        // Genera una dirección aleatoria lejos del jugador
        Vector3 direccion = Random.insideUnitSphere.normalized;

        // Elegimos una distancia entre "distanciaMin" y "radioSpawn"
        float distancia = Random.Range(distanciaMin, radioSpawn);

        // Calcula posición final respecto al jugador
        Vector3 posicion = Spawner.position + direccion * distancia;
        posicion.y = altura; // Mantiene altura (para mapas planos)

        return posicion;
    }

    // 🧪 Dibuja el rango del spawn en la escena
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioSpawn);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaMin);
    }
}
