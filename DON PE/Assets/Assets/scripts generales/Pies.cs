/*using UnityEngine;

public class Pies : MonoBehaviour
{
    [Header("Configuración")]
    public string tagTrigo = "Trigo";               // Tag del trigo que el duende destruye
    public GameObject prefabTierraSinPreparar;      // Prefab que se colocará debajo
    public float offsetY = -0.05f;                  // Pequeño ajuste de altura
    public float radioDeteccion = 0.4f;             // Radio para verificar si ya existe tierra

    [Header("Efectos opcionales")]
    public AudioClip sonidoPisada;
    public ParticleSystem particulasContacto;

    private AudioSource audioSrc;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    void OnTriggerStay(Collider other)
    {
        // Solo actúa si el duende está sobre donde había un trigo que fue destruido
        if (other.CompareTag(tagTrigo))
        {
            // Si el trigo sigue existiendo, no hacemos nada
            return;
        }

        // Detectar si ya hay una tierra sin preparar cerca (para no duplicar)
        Collider[] existentes = Physics.OverlapSphere(transform.position, radioDeteccion);
        foreach (var col in existentes)
        {
            if (col.gameObject.name.Contains("TierraSinPreparar"))
                return;
        }

        // Instanciar tierra sin preparar debajo de los pies
        if (prefabTierraSinPreparar != null)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
            Instantiate(prefabTierraSinPreparar, pos, Quaternion.identity);
        }

        // Sonido y partículas (opcionales)
        if (sonidoPisada && audioSrc)
            audioSrc.PlayOneShot(sonidoPisada);

        if (particulasContacto)
            Instantiate(particulasContacto, transform.position, Quaternion.identity);

        Debug.Log($"🌾 Tierra sin preparar creada debajo de {name}");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.7f, 0f, 0.4f);
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
    }
}
*/
/*using System.Collections.Generic;
using UnityEngine;

public class Pies : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Tag de los objetos de trigo a detectar.")]
    public string trigoTag = "trigo";

    [Tooltip("Prefab de tierra sin preparar.")]
    public GameObject tierraSinPrepararPrefab;

    [Tooltip("Distancia mínima para evitar duplicar tierra en el mismo lugar.")]
    public float radioChequeo = 0.5f;

    [Tooltip("Capa que representa la tierra sin preparar (opcional).")]
    public LayerMask capaTierra;

    // Mantiene registro de qué trigos ya fueron procesados este frame
    private HashSet<GameObject> trigosProcesados = new HashSet<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        // Solo actúa si es un trigo
        if (!other.CompareTag(trigoTag))
            return;

        // Evita procesar el mismo trigo varias veces
        if (trigosProcesados.Contains(other.gameObject))
            return;

        trigosProcesados.Add(other.gameObject);

        Vector3 pos = other.transform.position;

        // Evita duplicar tierra si ya hay una muy cerca
        bool existeTierra = Physics.CheckSphere(pos, radioChequeo, capaTierra);
        if (!existeTierra && tierraSinPrepararPrefab != null)
        {
            Instantiate(tierraSinPrepararPrefab, pos, Quaternion.identity);
        }

        // El trigo se destruye por el duende, así que no lo tocamos aquí
    }

    private void LateUpdate()
    {
        // Limpia la lista cada frame para evitar acumulación
        trigosProcesados.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioChequeo);
    }
}
*/
using System.Collections.Generic;
using UnityEngine;

public class Pies : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Tag de los objetos de trigo a detectar.")]
    public string trigoTag = "Trigo";

    [Tooltip("Prefab de tierra sin preparar.")]
    public GameObject tierraSinPrepararPrefab;

    [Tooltip("Distancia mínima para evitar duplicar tierra en el mismo lugar.")]
    public float radioChequeo = 0.5f;

    [Tooltip("Capa que representa la tierra sin preparar (opcional).")]
    public LayerMask capaTierra;

    [Tooltip("Altura fija para el prefab instanciado.")]
    public float alturaTierra = 0.95f;

    private HashSet<GameObject> trigosProcesados = new HashSet<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(trigoTag))
            return;

        if (trigosProcesados.Contains(other.gameObject))
            return;

        trigosProcesados.Add(other.gameObject);

        Vector3 pos = other.transform.position;
        pos.y = alturaTierra; // Forzar altura exacta

        bool existeTierra = Physics.CheckSphere(pos, radioChequeo, capaTierra);
        if (!existeTierra && tierraSinPrepararPrefab != null)
        {
            Instantiate(tierraSinPrepararPrefab, pos, Quaternion.identity);
        }
    }

    private void LateUpdate()
    {
        trigosProcesados.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioChequeo);
    }
}
