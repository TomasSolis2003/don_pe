/*using UnityEngine;

public class CamionRegador : MonoBehaviour
{
    [Header("Configuraci�n")]
    public GameObject tierraMojadaPrefab; // Prefab que representa la tierra mojada

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto tiene el tag "tierra preparada"
        if (other.CompareTag("tierra preparada"))
        {
            Tierra tierra = other.GetComponent<Tierra>();
            if (tierra != null)
            {
                tierra.MojarTierra(tierraMojadaPrefab); // Cambia el estado de la tierra a mojada
            }
        }
    }
}
*/
/*using UnityEngine;

public class CamionRegador : MonoBehaviour
{
    [Header("Configuraci�n")]
    public Collider areaDeRiego; // Collider asignado manualmente en el inspector
    public GameObject tierraMojadaPrefab; // Prefab para tierra mojada

    private void Start()
    {
        if (areaDeRiego == null)
        {
            Debug.LogError("Debes asignar un Collider al campo 'areaDeRiego' en el Inspector.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra en contacto tiene el tag "tierra preparada"
        if (other.CompareTag("tierra preparada"))
        {
            Tierra tierra = other.GetComponent<Tierra>();
            if (tierra != null)
            {
                tierra.MojarTierra(tierraMojadaPrefab); // Cambia el estado de la tierra a mojada
            }
        }
    }
}
*/
using UnityEngine;

public class CamionRegador : MonoBehaviour
{
    [Header("Configuraci�n de riego")]
    public BoxCollider areaDeRiego; // BoxCollider que define el �rea de riego
    public GameObject tierraPreparadaMojadaPrefab; // Prefab para "tierra preparada-MOJADA"

    private void Start()
    {
        // Validar que el �rea de riego est� configurada
        if (areaDeRiego == null)
        {
            Debug.LogError("El BoxCollider para el �rea de riego no est� asignado. Por favor, config�ralo en el Inspector.");
        }

        // Validar que el prefab est� configurado
        if (tierraPreparadaMojadaPrefab == null)
        {
            Debug.LogError("El prefab para tierra preparada-MOJADA no est� asignado. Por favor, config�ralo en el Inspector.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Validar que el �rea de riego est� configurada antes de proceder
        if (areaDeRiego != null && other.CompareTag("tierra preparada"))
        {
            // Reemplazar el objeto con el prefab de "tierra preparada-MOJADA"
            Transform tierraTransform = other.transform;
            Instantiate(tierraPreparadaMojadaPrefab, tierraTransform.position, tierraTransform.rotation, tierraTransform.parent);

            // Destruir el objeto original
            Destroy(other.gameObject);
        }
    }
}
