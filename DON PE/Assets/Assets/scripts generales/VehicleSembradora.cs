using UnityEngine;

public class VehicleSembradora : MonoBehaviour
{
    [Header("Configuración de siembra")]
    [SerializeField] private string tagTierra = "tierra preparada";
    [SerializeField] private GameObject prefabSemilla;   // Prefab del trigo en etapa inicial
    [SerializeField] private float distanciaSpawn = 1.5f; // Separación mínima entre siembras
    [SerializeField] private float alturaSpawn = 0.1f;    // Altura sobre el terreno

    private Vector3 ultimaPosicionSpawn;

    private void Start()
    {
        ultimaPosicionSpawn = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tagTierra)) return;

        float distancia = Vector3.Distance(transform.position, ultimaPosicionSpawn);
        if (distancia < distanciaSpawn) return; // evita plantar demasiado cerca

        PlantarSemilla(other.transform.position);
    }

    private void PlantarSemilla(Vector3 posicion)
    {
        if (prefabSemilla == null)
        {
            Debug.LogWarning("🌾 No se asignó el prefab de semilla.");
            return;
        }

        Vector3 pos = new Vector3(posicion.x, posicion.y + alturaSpawn, posicion.z);
        Instantiate(prefabSemilla, pos, Quaternion.identity);
        ultimaPosicionSpawn = pos;

        Debug.Log("🌱 Semilla plantada en " + pos);
    }
}
