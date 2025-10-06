/*using UnityEngine;

public class PlowMechanic : MonoBehaviour
{
    [Header("Tags")]
    [Tooltip("Tag del vehículo arador.")]
    public string plowTag = "arador";

    [Tooltip("Tag de la tierra sin preparar.")]
    public string unpreparedLandTag = "tierra sin preparar";

    [Header("Prefab de tierra preparada")]
    [Tooltip("Prefab que reemplazará la tierra sin preparar.")]
    public GameObject preparedLandPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        // Verificar si el objeto que colisionó tiene el tag de tierra sin preparar
        if (collision.gameObject.CompareTag(unpreparedLandTag))
        {
            // Obtener la posición del objeto de tierra sin preparar
            Vector3 position = collision.transform.position;
            Quaternion rotation = collision.transform.rotation;

            // Instanciar el prefab de tierra preparada en la misma posición y rotación
            Instantiate(preparedLandPrefab, position, rotation);

            // Destruir el objeto de tierra sin preparar para liberar recursos
            Destroy(collision.gameObject);
        }
    }
}
*/
/*using UnityEngine;

public class PlowMechanic : MonoBehaviour
{
    [Header("Tags")]
    [Tooltip("Tag del vehículo arador.")]
    public string plowTag = "arador";

    [Tooltip("Tag de la tierra sin preparar.")]
    public string unpreparedLandTag = "tierra sin preparar";

    [Header("Prefab de tierra preparada")]
    [Tooltip("Prefab que reemplazará la tierra sin preparar.")]
    public GameObject preparedLandPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        // Verificar si el objeto que colisionó tiene el tag de tierra sin preparar
        if (collision.gameObject.CompareTag(unpreparedLandTag) && preparedLandPrefab != null)
        {
            // Obtener la posición del objeto de tierra sin preparar
            Vector3 position = collision.transform.position;
            Quaternion rotation = collision.transform.rotation;

            // Instanciar el prefab de tierra preparada en la misma posición y rotación
            Instantiate(preparedLandPrefab, position, rotation);

            // Destruir el objeto de tierra sin preparar para liberar recursos
            Destroy(collision.gameObject);
        }
    }
}
*/
using UnityEngine;

public class PlowMechanic : MonoBehaviour
{
    [Header("Tags")]
    [Tooltip("Tag del vehículo arador.")]
    public string plowTag = "arador";

    [Tooltip("Tag de la tierra sin preparar.")]
    public string unpreparedLandTag = "tierra sin preparar";

    [Header("Prefab de tierra preparada")]
    [Tooltip("Prefab que reemplazará la tierra sin preparar.")]
    public GameObject preparedLandPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colisión detectada con: " + collision.gameObject.name);
        // Verificar si el objeto que colisionó tiene el tag de tierra sin preparar
        if (collision.gameObject.CompareTag(unpreparedLandTag) && preparedLandPrefab != null)
        {
            Debug.Log("Colisión con tierra sin preparar detectada.");
            // Obtener la posición del objeto de tierra sin preparar
            Vector3 position = collision.transform.position;
            Quaternion rotation = collision.transform.rotation;

            // Instanciar el prefab de tierra preparada en la misma posición y rotación
            Instantiate(preparedLandPrefab, position, rotation);

            // Destruir el objeto de tierra sin preparar para liberar recursos
            Destroy(collision.gameObject);
        }
        else
        {
            Debug.Log("Colisión no válida o prefab no asignado.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detectado con: " + other.gameObject.name);
        // Verificar si el objeto que colisionó tiene el tag de tierra sin preparar
        if (other.CompareTag(unpreparedLandTag) && preparedLandPrefab != null)
        {
            Debug.Log("Trigger con tierra sin preparar detectado.");
            // Obtener la posición del objeto de tierra sin preparar
            Vector3 position = other.transform.position;
            Quaternion rotation = other.transform.rotation;

            // Instanciar el prefab de tierra preparada en la misma posición y rotación
            Instantiate(preparedLandPrefab, position, rotation);

            // Destruir el objeto de tierra sin preparar para liberar recursos
            Destroy(other.gameObject);
        }
        else
        {
            Debug.Log("Trigger no válido o prefab no asignado.");
        }
    }
}
