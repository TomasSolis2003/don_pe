using UnityEngine;
using UnityEngine.EventSystems; // Para UI

public class ConstruccionJugador : MonoBehaviour
{
    [Header("Referencias")]
    public Camera camaraJugador;
    public InventarioJugador inventario;
    public GameObject panelConstruccion;

    [Header("Construcción")]
    public LayerMask capaSuelo;          // Dónde se puede construir
    public float distanciaColocacion = 5f;
    public KeyCode teclaConstruir = KeyCode.Mouse0;

    private GameObject previewActual;
    private GameObject prefabSeleccionado;
    private float rotacionActual = 0f;

    void Update()
    {
        // Solo si hay preview activo
        if (previewActual != null)
        {
            MoverPreview();
            RotarPreview();

            if (Input.GetKeyDown(teclaConstruir))
            {
                ColocarConstruccion();
            }
        }
    }

    // 🔹 Seleccionado desde UI (botón del panel)
    public void SeleccionarConstruccion(GameObject prefab)
    {
        if (previewActual != null) Destroy(previewActual);

        prefabSeleccionado = prefab;
        previewActual = Instantiate(prefabSeleccionado);
        previewActual.GetComponent<Collider>().enabled = false; // Fantasma sin colisión
        SetMaterialPreview(previewActual, new Color(0, 1, 0, 0.5f)); // Verde transparente
    }

    void MoverPreview()
    {
        Ray rayo = camaraJugador.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Centro de la cámara
        if (Physics.Raycast(rayo, out RaycastHit hit, distanciaColocacion, capaSuelo))
        {
            previewActual.transform.position = hit.point;
            previewActual.transform.rotation = Quaternion.Euler(0, rotacionActual, 0);
        }
    }

    void RotarPreview()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            rotacionActual += scroll * 90f; // Gira de 90° en 90°
        }
    }

    void ColocarConstruccion()
    {
        // Chequear recursos
        if (inventario.GastarTroncos(5)) // Ejemplo: cuesta 5 troncos
        {
            GameObject objetoFinal = Instantiate(prefabSeleccionado,
                previewActual.transform.position,
                previewActual.transform.rotation);

            // Restaurar materiales originales
            SetMaterialPreview(objetoFinal, Color.white);
        }
        else
        {
            Debug.Log("No tienes suficientes troncos");
        }
    }

    void SetMaterialPreview(GameObject go, Color color)
    {
        Renderer[] rends = go.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
        {
            foreach (Material m in r.materials)
            {
                m.color = color;
            }
        }
    }
}
