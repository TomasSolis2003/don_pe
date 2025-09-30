/*using UnityEngine;
using UnityEngine.UI;

public class PanelConstruccion : MonoBehaviour
{
    [System.Serializable]
    public class Estructura
    {
        public string nombre;
        public GameObject prefab;
        public Sprite icono;
        public int costoTroncos;
    }

    [Header("Configuración")]
    public Estructura[] estructuras;
    public GameObject botonPrefab; // Prefab de botón (UI) con Image + Text
    public Transform contenedorBotones; // Donde se crearán los botones
    public InventarioJugador inventario; // Referencia al inventario del jugador

    void Start()
    {
        // Generar los botones en el panel
        foreach (var estructura in estructuras)
        {
            GameObject botonGO = Instantiate(botonPrefab, contenedorBotones);
            Button boton = botonGO.GetComponent<Button>();

            // Cambiar imagen y texto
            botonGO.GetComponentInChildren<Image>().sprite = estructura.icono;
            botonGO.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = estructura.nombre + " (" + estructura.costoTroncos + ")";

            // Guardar referencia para el evento
            Estructura refEstructura = estructura;
            boton.onClick.AddListener(() => SeleccionarEstructura(refEstructura));
        }
    }

    void SeleccionarEstructura(Estructura estructura)
    {
        if (inventario.GastarTroncos(estructura.costoTroncos))
        {
            inventario.IniciarConstruccion(estructura.prefab);
        }
        else
        {
            Debug.Log("No tienes suficientes troncos para construir: " + estructura.nombre);
        }
    }
}
*/
/*using UnityEngine;
using UnityEngine.UI;

public class PanelConstruccion : MonoBehaviour
{
    [System.Serializable]
    public class Estructura
    {
        public string nombre;
        public GameObject prefab;
        public Sprite icono;
        public int costoTroncos;
    }

    [Header("Configuración")]
    public Estructura[] estructuras;
    public GameObject botonPrefab;          // Prefab con BotonEstructuraUI
    public Transform contenedorBotones;     // Parent de los botones
    public InventarioJugador inventario;    // Referencia al inventario

    bool _construido;

    void OnEnable()
    {
        // Si reabrís el panel, reconstruí para reflejar costos/stock
        Reconstruir();
    }

    void Start()
    {
        if (!_construido) Reconstruir();
    }

    void Reconstruir()
    {
        if (contenedorBotones == null || botonPrefab == null)
        {
            Debug.LogError("PanelConstruccion: faltan referencias (contenedorBotones o botonPrefab).");
            return;
        }

        // Limpiar lo anterior para evitar listeners duplicados
        for (int i = contenedorBotones.childCount - 1; i >= 0; i--)
            Destroy(contenedorBotones.GetChild(i).gameObject);

        // Crear uno por estructura
        foreach (var estructura in estructuras)
        {
            var go = Instantiate(botonPrefab, contenedorBotones);
            var ui = go.GetComponent<BotonEstructuraUI>();
            var btn = ui ? ui.button : go.GetComponent<Button>();

            if (ui) ui.Set(estructura.icono, $"{estructura.nombre} ({estructura.costoTroncos})");

            // Captura local segura para el listener
            Estructura dato = estructura;

            if (btn)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => OnClickEstructura(dato));
            }
        }

        _construido = true;
    }

    void OnClickEstructura(Estructura estructura)
    {
        if (estructura == null || estructura.prefab == null)
        {
            Debug.LogWarning("PanelConstruccion: estructura o prefab nulo.");
            return;
        }
        if (inventario == null)
        {
            Debug.LogError("PanelConstruccion: falta referencia a InventarioJugador.");
            return;
        }

        // OJO: Gastar al seleccionar vs. al colocar
        // Si preferís cobrar al confirmar colocación, mové el gasto a ColocarEstructura().
        if (inventario.GastarTroncos(estructura.costoTroncos))
        {
            inventario.IniciarConstruccion(estructura.prefab);
        }
        else
        {
            Debug.Log($"No tenés suficientes troncos para: {estructura.nombre}");
        }
    }
}
*/
using UnityEngine;

public class PanelConstruccion : MonoBehaviour
{
    public InventarioJugador inventario;
    public BotonEstructuraUI[] botones;

    void Start()
    {
        foreach (var b in botones)
        {
            if (b != null)
                b.Inicializar(this);
        }
    }

    public void SeleccionarEstructura(BotonEstructuraUI boton)
    {
        if (!inventario) return;

        // igual que antes: mejor gastar al colocar, pero si querés gastar acá, cambiás esto
        if (inventario.GastarTroncos(boton.costoTroncos))
        {
            inventario.IniciarConstruccion(boton.prefab);
        }
        else
        {
            Debug.Log($"No tenés troncos suficientes para {boton.nombre}");
        }
    }
}
