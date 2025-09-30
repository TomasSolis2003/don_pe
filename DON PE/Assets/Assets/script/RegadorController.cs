using System.Collections;
using UnityEngine;

public class RegadorController : MonoBehaviour
{
    [Header("Prefabs de Estados de la Tierra")]
    public GameObject tierraPreparadaHumedaPrefab;
    public GameObject tierraPreparadaPrefab;
    public GameObject tierraSecaPrefab;

    [Header("BoxCollider del Regador")]
    public BoxCollider regadorCollider;

    private void Start()
    {
        if (regadorCollider == null)
        {
            regadorCollider = GetComponent<BoxCollider>();
            if (regadorCollider == null)
            {
                Debug.LogError("El BoxCollider no está asignado ni encontrado en el GameObject.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si colisiona con "tierra preparada"
        if (other.CompareTag("tierra preparada"))
        {
            // Obtener la posición y rotación del objeto colisionado
            Vector3 position = other.transform.position;
            Quaternion rotation = other.transform.rotation;

            // Destruir el objeto colisionado
            Destroy(other.gameObject);

            // Instanciar el primer estado (tierra preparada húmeda)
            GameObject tierraHumeda = Instantiate(tierraPreparadaHumedaPrefab, position, rotation);

            // Iniciar el ciclo de estados
            StartCoroutine(CambiarEstados(tierraHumeda, position, rotation));
        }
    }

    private IEnumerator CambiarEstados(GameObject tierraHumeda, Vector3 position, Quaternion rotation)
    {
        // 2 minutos como tierra preparada húmeda
        yield return new WaitForSeconds(120);
        Destroy(tierraHumeda);

        // Instanciar tierra preparada
        GameObject tierraPreparada = Instantiate(tierraPreparadaPrefab, position, rotation);

        // 2 minutos como tierra preparada
        yield return new WaitForSeconds(120);
        Destroy(tierraPreparada);

        // Instanciar tierra seca
        GameObject tierraSeca = Instantiate(tierraSecaPrefab, position, rotation);

        // 1 minuto como tierra seca
        yield return new WaitForSeconds(60);
        Destroy(tierraSeca);
    }
}
