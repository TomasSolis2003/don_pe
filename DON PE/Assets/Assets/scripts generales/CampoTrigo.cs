/*using UnityEngine;

public class CampoTrigo : MonoBehaviour
{
    [Header("Configuración del campo")]
    public GameObject prefabTrigo;
    public int ancho = 10;          // número de filas
    public int largo = 10;          // número de columnas
    public float separacion = 1.5f; // distancia entre plantas

    [Header("Altura y variación visual")]
    public float variacionAltura = 0.2f;   // altura aleatoria para evitar que se vean idénticos
    public float variacionRotacion = 15f;  // rotación aleatoria en Y

    [Header("Auto-generar al iniciar")]
    public bool generarAlInicio = true;

    void Start()
    {
        if (generarAlInicio)
            GenerarCampo();
    }

    [ContextMenu("Generar campo de trigo")]
    public void GenerarCampo()
    {
        if (prefabTrigo == null)
        {
            Debug.LogError("No se asignó el prefab de trigo.");
            return;
        }

        // Borrar trigos previos si se regenera el campo
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        Vector3 origen = transform.position;
        float offsetX = (ancho - 1) * separacion * 0.5f;
        float offsetZ = (largo - 1) * separacion * 0.5f;

        for (int x = 0; x < ancho; x++)
        {
            for (int z = 0; z < largo; z++)
            {
                Vector3 pos = new Vector3(
                    origen.x + (x * separacion) - offsetX,
                    origen.y,
                    origen.z + (z * separacion) - offsetZ
                );

                Quaternion rot = Quaternion.Euler(0f, Random.Range(-variacionRotacion, variacionRotacion), 0f);
                GameObject trigo = Instantiate(prefabTrigo, pos, rot, transform);

                // ajustar pequeña variación de escala para naturalidad
                float factorAltura = 1f + Random.Range(-variacionAltura, variacionAltura);
                trigo.transform.localScale = new Vector3(1f, factorAltura, 1f);
            }
        }
    }
}
*/
using UnityEngine;

public class CampoTrigo : MonoBehaviour
{
    [Header("Configuración del campo")]
    public GameObject prefabTrigo;
    public int ancho = 10;          // número de filas
    public int largo = 10;          // número de columnas
    public float separacion = 1.5f; // distancia entre plantas

    [Header("Altura y variación visual")]
    public float variacionAltura = 0.2f;
    public float variacionRotacion = 15f;

    [Header("Auto-generar al iniciar")]
    public bool generarAlInicio = true;

    [Header("Gizmos")]
    public Color colorGizmoCampo = new Color(1f, 0.9f, 0f, 0.25f);
    public Color colorGizmoPuntos = Color.yellow;
    public bool mostrarGizmos = true;

    void Start()
    {
        if (generarAlInicio)
            GenerarCampo();
    }

    [ContextMenu("Generar campo de trigo")]
    public void GenerarCampo()
    {
        if (prefabTrigo == null)
        {
            Debug.LogError("No se asignó el prefab de trigo.");
            return;
        }

        // Borrar trigos previos si se regenera el campo
        for (int i = transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(transform.GetChild(i).gameObject);

        Vector3 origen = transform.position;
        float offsetX = (ancho - 1) * separacion * 0.5f;
        float offsetZ = (largo - 1) * separacion * 0.5f;

        for (int x = 0; x < ancho; x++)
        {
            for (int z = 0; z < largo; z++)
            {
                Vector3 pos = new Vector3(
                    origen.x + (x * separacion) - offsetX,
                    origen.y,
                    origen.z + (z * separacion) - offsetZ
                );

                Quaternion rot = Quaternion.Euler(0f, Random.Range(-variacionRotacion, variacionRotacion), 0f);
                GameObject trigo = Instantiate(prefabTrigo, pos, rot, transform);

                float factorAltura = 1f + Random.Range(-variacionAltura, variacionAltura);
                trigo.transform.localScale = new Vector3(1f, factorAltura, 1f);
            }
        }
    }

    // --- GIZMOS VISUALES ---
    void OnDrawGizmosSelected()
    {
        if (!mostrarGizmos) return;

        Gizmos.color = colorGizmoCampo;

        // dibujar borde del área
        float anchoTotal = (ancho - 1) * separacion;
        float largoTotal = (largo - 1) * separacion;
        Vector3 centro = transform.position;
        Vector3 size = new Vector3(anchoTotal, 0.1f, largoTotal);

        Gizmos.DrawWireCube(centro, size);

        // dibujar puntos individuales donde irá cada planta
        Gizmos.color = colorGizmoPuntos;
        float offsetX = (ancho - 1) * separacion * 0.5f;
        float offsetZ = (largo - 1) * separacion * 0.5f;

        for (int x = 0; x < ancho; x++)
        {
            for (int z = 0; z < largo; z++)
            {
                Vector3 pos = new Vector3(
                    transform.position.x + (x * separacion) - offsetX,
                    transform.position.y,
                    transform.position.z + (z * separacion) - offsetZ
                );
                Gizmos.DrawSphere(pos, 0.05f);
            }
        }
    }
}
