

/*using TMPro;
using UnityEngine;

public class InventarioJugador : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI troncosTexto;
    [SerializeField] private GameObject panelConstruccion;

    [Header("UI Botones Construcción")]
    [SerializeField] private GameObject[] botonesConstruccion;

    [Header("Inventario de Objetos")]
    [SerializeField] private GameObject hacha;
    private GameObject objetoActual;

    [Header("Inputs")]
    [SerializeField] private KeyCode teclaPanel = KeyCode.P;
    [SerializeField] private KeyCode teclaHacha = KeyCode.Alpha1;

    [Header("Golpe con hacha")]
    [SerializeField] private float rangoGolpe = 2f;     // Distancia máxima de impacto
    [SerializeField] private LayerMask capaArboles;     // Capa para detectar árboles
    [SerializeField] private float cdAtaque = 0.45f;    // Cooldown simple del swing
    private bool puedeAtacar = true;
    private Coroutine swingCR;

    // Pose base del hacha (para no “volver torcida” si guardás a mitad del swing)
    private Vector3 hachaPos0;
    private Quaternion hachaRot0;

    [Header("Construcción (preview / colocación)")]
    [SerializeField] private Transform puntoColocacion;   // Empty delante del jugador (opcional)
    [SerializeField] private float distanciaPreview = 3f; // Fallback si no hay puntoColocacion
    [SerializeField] private float minYPlacement = 2.5f;  // No permitir y < 2.5
    [SerializeField] public GameObject previewActual;
    private GameObject prefabSeleccionado;

    // Cache de cámara para evitar Camera.main por frame
    private Camera cam;

    // --- Recursos ---
    private int troncos = 0;

    // ----------------------------------------------------------
    void Start()
    {
        ActualizarUI();

        if (panelConstruccion != null)
            panelConstruccion.SetActive(false);

        cam = Camera.main;

        if (hacha != null)
        {
            hachaPos0 = hacha.transform.localPosition;
            hachaRot0 = hacha.transform.localRotation;
            hacha.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Por si la cámara cambia en runtime
        if (cam == null) cam = Camera.main;

        ManejarPanelConstruccion();
        ManejarInventario();
        ManejarPreviewConstruccion();
    }

    // ================== PANEL ==================
    void ManejarPanelConstruccion()
    {
        if (Input.GetKeyDown(teclaPanel))
        {
            if (panelConstruccion == null) return;

            bool nuevoEstado = !panelConstruccion.activeSelf;
            panelConstruccion.SetActive(nuevoEstado);

            if (botonesConstruccion != null)
            {
                foreach (var boton in botonesConstruccion)
                    if (boton != null) boton.SetActive(nuevoEstado);
            }

            if (nuevoEstado)
            {
                // Evitar clics accidentales mientras navegas UI
                GuardarObjeto();

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    // ============ INVENTARIO / HACHA ============
    void ManejarInventario()
    {
        // Toggle del hacha con la tecla asignada
        if (Input.GetKeyDown(teclaHacha))
        {
            if (objetoActual == hacha) GuardarObjeto();
            else EquiparObjeto(hacha);
        }

        // Ataque solo si hay hacha equipada
        if (objetoActual == hacha && Input.GetMouseButtonDown(0))
        {
            if (puedeAtacar && swingCR == null)
                swingCR = StartCoroutine(AnimacionAtaqueHacha());
        }
    }

    void EquiparObjeto(GameObject objeto)
    {
        if (objetoActual != null)
            objetoActual.SetActive(false);

        objetoActual = objeto;

        if (objetoActual != null)
        {
            // Asegurar pose base del hacha al equipar
            if (objetoActual == hacha)
            {
                hacha.transform.localPosition = hachaPos0;
                hacha.transform.localRotation = hachaRot0;
            }
            objetoActual.SetActive(true);
        }
    }

    void GuardarObjeto()
    {
        if (objetoActual != null)
        {
            // Detener SOLO el swing si estaba activo
            if (swingCR != null)
            {
                StopCoroutine(swingCR);
                swingCR = null;
            }

            // Resetear pose del hacha para la próxima vez
            if (objetoActual == hacha)
            {
                hacha.transform.localPosition = hachaPos0;
                hacha.transform.localRotation = hachaRot0;
            }

            puedeAtacar = true;
            objetoActual.SetActive(false);
            objetoActual = null;
        }
    }

    System.Collections.IEnumerator AnimacionAtaqueHacha()
    {
        if (hacha == null) yield break;

        puedeAtacar = false;

        // Si durante el golpe guardan el hacha, salir prolijo
        if (objetoActual != hacha) { puedeAtacar = true; swingCR = null; yield break; }

        // Duraciones
        float duracionGolpe = 0.25f;
        float duracionVuelta = 0.30f;

        // Tomar como base la pose cacheada (consistente)
        Vector3 posInicial = hachaPos0;
        Quaternion rotInicial = hachaRot0;

        Vector3 posAtras = posInicial + new Vector3(0f, 0f, -0.2f);
        Quaternion rotAtras = Quaternion.Euler(rotInicial.eulerAngles + new Vector3(-30f, 0f, 0f));

        Vector3 posAdelante = posInicial + new Vector3(0f, 0f, 0.30f);
        Quaternion rotAdelante = Quaternion.Euler(rotInicial.eulerAngles + new Vector3(40f, 0f, 0f));

        float t = 0f;

        // Preparación hacia atrás
        while (t < duracionGolpe * 0.5f)
        {
            if (objetoActual != hacha) { puedeAtacar = true; swingCR = null; yield break; }
            hacha.transform.localPosition = Vector3.Lerp(posInicial, posAtras, t / (duracionGolpe * 0.5f));
            hacha.transform.localRotation = Quaternion.Slerp(rotInicial, rotAtras, t / (duracionGolpe * 0.5f));
            t += Time.deltaTime;
            yield return null;
        }

        // Golpe hacia adelante
        t = 0f;
        while (t < duracionGolpe * 0.5f)
        {
            if (objetoActual != hacha) { puedeAtacar = true; swingCR = null; yield break; }
            hacha.transform.localPosition = Vector3.Lerp(posAtras, posAdelante, t / (duracionGolpe * 0.5f));
            hacha.transform.localRotation = Quaternion.Slerp(rotAtras, rotAdelante, t / (duracionGolpe * 0.5f));
            t += Time.deltaTime;
            yield return null;
        }

        // Impacto
        DetectarImpacto();

        // Vuelta a reposo
        t = 0f;
        while (t < duracionVuelta)
        {
            if (objetoActual != hacha) { puedeAtacar = true; swingCR = null; yield break; }
            hacha.transform.localPosition = Vector3.Lerp(posAdelante, posInicial, t / duracionVuelta);
            hacha.transform.localRotation = Quaternion.Slerp(rotAdelante, rotInicial, t / duracionVuelta);
            t += Time.deltaTime;
            yield return null;
        }

        // Pose exacta base
        hacha.transform.localPosition = hachaPos0;
        hacha.transform.localRotation = hachaRot0;

        // Pequeño CD para no “metrallear” clics
        yield return new WaitForSeconds(cdAtaque);
        puedeAtacar = true;
        swingCR = null;
    }

    void DetectarImpacto()
    {
        if (cam == null) return;

        Ray rayo = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(rayo, out RaycastHit hit, rangoGolpe, capaArboles, QueryTriggerInteraction.Ignore))
        {
            var arbol = hit.collider.GetComponent<Arbol>();
            if (arbol != null)
                arbol.RecibirGolpe();
        }
    }

    // ========== PREVIEW / COLOCACIÓN ==========
    void ManejarPreviewConstruccion()
    {
        if (previewActual == null) return;

        // Posición objetivo: punto de colocación si existe; si no, frente a cámara/jugador
        Vector3 targetPos;
        if (puntoColocacion != null) targetPos = puntoColocacion.position;
        else if (cam != null) targetPos = cam.transform.position + cam.transform.forward * distanciaPreview;
        else targetPos = transform.position + transform.forward * distanciaPreview;

        // clamp en Y para no bajar del mínimo
        if (targetPos.y < minYPlacement) targetPos.y = minYPlacement;

        previewActual.transform.position = targetPos;

        // rotación con rueda del mouse
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.0001f)
            previewActual.transform.Rotate(Vector3.up, scroll * 100f, Space.World);

        // confirmar colocación
        if (Input.GetMouseButtonDown(0))
            ColocarEstructura();
    }

    void ColocarEstructura()
    {
        if (previewActual == null || prefabSeleccionado == null) return;

        Vector3 spawnPos = previewActual.transform.position;
        if (spawnPos.y < minYPlacement) spawnPos.y = minYPlacement;

        Instantiate(prefabSeleccionado, spawnPos, previewActual.transform.rotation);

        Destroy(previewActual);
        previewActual = null;
        prefabSeleccionado = null;
    }

    public void IniciarConstruccion(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("InventarioJugador.IniciarConstruccion: prefab null");
            return;
        }

        prefabSeleccionado = prefab;

        if (previewActual != null)
            Destroy(previewActual);

        // Crear preview y dejarlo semi-transparente
        previewActual = Instantiate(prefabSeleccionado);

        // Desactivar colliders del preview para evitar interferencias
        foreach (var c in previewActual.GetComponentsInChildren<Collider>())
            c.enabled = false;

        // Materiales semi-transparentes (accede a instancias)
        foreach (var r in previewActual.GetComponentsInChildren<Renderer>())
        {
            var mats = r.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                Color baseCol = mats[i].color;
                mats[i].color = new Color(baseCol.r, baseCol.g, baseCol.b, 0.5f);
            }
        }

        // Posición inicial del preview
        if (puntoColocacion != null)
            previewActual.transform.position = puntoColocacion.position;
        else if (cam != null)
            previewActual.transform.position = cam.transform.position + cam.transform.forward * distanciaPreview;
        else
            previewActual.transform.position = transform.position + transform.forward * distanciaPreview;

        // Asegurar mínimo en Y
        Vector3 p = previewActual.transform.position;
        if (p.y < minYPlacement) p.y = minYPlacement;
        previewActual.transform.position = p;
    }

    // ================= TRONCOS =================
    public void AgregarTronco(int cantidad)
    {
        troncos += cantidad;
        ActualizarUI();
    }

    public bool GastarTroncos(int cantidad)
    {
        if (troncos >= cantidad)
        {
            troncos -= cantidad;
            ActualizarUI();
            return true;
        }
        return false;
    }

    void ActualizarUI()
    {
        if (troncosTexto != null)
            troncosTexto.text = "Troncos: " + troncos;
    }
}
*/
using TMPro;
using UnityEngine;

public class InventarioJugador : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI troncosTexto;
    [SerializeField] private GameObject panelConstruccion;

    [Header("UI Botones Construcción")]
    [SerializeField] private GameObject[] botonesConstruccion;

    [Header("Inventario de Objetos")]
    [SerializeField] private GameObject hacha;
    [SerializeField] private GameObject machete; // NUEVO arma aparte
    private GameObject objetoActual;

    [Header("Inputs")]
    [SerializeField] private KeyCode teclaPanel = KeyCode.P;
    [SerializeField] private KeyCode teclaHacha = KeyCode.Alpha1;
    [SerializeField] private KeyCode teclaMachete = KeyCode.Alpha2; // NUEVO

    [Header("Golpe con hacha")]
    [SerializeField] private float rangoGolpe = 2f;
    [SerializeField] private LayerMask capaArboles;
    [SerializeField] private float cdAtaque = 0.45f;
    private bool puedeAtacar = true;
    private Coroutine swingCR;

    private Vector3 hachaPos0;
    private Quaternion hachaRot0;

    [Header("Construcción (preview / colocación)")]
    [SerializeField] private Transform puntoColocacion;
    [SerializeField] private float distanciaPreview = 3f;
    [SerializeField] private float minYPlacement = 2.5f;
    [SerializeField] public GameObject previewActual;
    private GameObject prefabSeleccionado;

    private Camera cam;
    private int troncos = 0;

    // ----------------------------------------------------------
    void Start()
    {
        ActualizarUI();

        if (panelConstruccion != null)
            panelConstruccion.SetActive(false);

        cam = Camera.main;

        if (hacha != null)
        {
            hachaPos0 = hacha.transform.localPosition;
            hachaRot0 = hacha.transform.localRotation;
            hacha.SetActive(false);
        }

        if (machete != null) machete.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (cam == null) cam = Camera.main;

        ManejarPanelConstruccion();
        ManejarInventario();
        ManejarPreviewConstruccion();
    }

    // ================== PANEL ==================
    void ManejarPanelConstruccion()
    {
        if (Input.GetKeyDown(teclaPanel))
        {
            if (panelConstruccion == null) return;

            bool nuevoEstado = !panelConstruccion.activeSelf;
            panelConstruccion.SetActive(nuevoEstado);

            if (botonesConstruccion != null)
            {
                foreach (var boton in botonesConstruccion)
                    if (boton != null) boton.SetActive(nuevoEstado);
            }

            if (nuevoEstado)
            {
                GuardarObjeto();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    // ============ INVENTARIO ============
    void ManejarInventario()
    {
        // Toggle hacha
        if (Input.GetKeyDown(teclaHacha))
            ToggleObjeto(hacha);

        // Toggle machete
        if (Input.GetKeyDown(teclaMachete))
            ToggleObjeto(machete);

        // Ataque solo si hay hacha equipada
        if (objetoActual == hacha && Input.GetMouseButtonDown(0))
        {
            if (puedeAtacar && swingCR == null)
                swingCR = StartCoroutine(AnimacionAtaqueHacha());
        }
    }

    void ToggleObjeto(GameObject objeto)
    {
        if (objetoActual == objeto)
            GuardarObjeto();
        else
            EquiparObjeto(objeto);
    }

    void EquiparObjeto(GameObject objeto)
    {
        if (objetoActual != null)
            objetoActual.SetActive(false);

        objetoActual = objeto;

        if (objetoActual != null)
        {
            if (objetoActual == hacha)
            {
                hacha.transform.localPosition = hachaPos0;
                hacha.transform.localRotation = hachaRot0;
            }
            objetoActual.SetActive(true);
        }
    }

    void GuardarObjeto()
    {
        if (objetoActual != null)
        {
            if (swingCR != null)
            {
                StopCoroutine(swingCR);
                swingCR = null;
            }

            if (objetoActual == hacha)
            {
                hacha.transform.localPosition = hachaPos0;
                hacha.transform.localRotation = hachaRot0;
            }

            puedeAtacar = true;
            objetoActual.SetActive(false);
            objetoActual = null;
        }
    }

    System.Collections.IEnumerator AnimacionAtaqueHacha()
    {
        if (hacha == null) yield break;

        puedeAtacar = false;

        if (objetoActual != hacha) { puedeAtacar = true; swingCR = null; yield break; }

        float duracionGolpe = 0.25f;
        float duracionVuelta = 0.30f;

        Vector3 posInicial = hachaPos0;
        Quaternion rotInicial = hachaRot0;

        Vector3 posAtras = posInicial + new Vector3(0f, 0f, -0.2f);
        Quaternion rotAtras = Quaternion.Euler(rotInicial.eulerAngles + new Vector3(-30f, 0f, 0f));

        Vector3 posAdelante = posInicial + new Vector3(0f, 0f, 0.30f);
        Quaternion rotAdelante = Quaternion.Euler(rotInicial.eulerAngles + new Vector3(40f, 0f, 0f));

        float t = 0f;

        while (t < duracionGolpe * 0.5f)
        {
            if (objetoActual != hacha) { puedeAtacar = true; swingCR = null; yield break; }
            hacha.transform.localPosition = Vector3.Lerp(posInicial, posAtras, t / (duracionGolpe * 0.5f));
            hacha.transform.localRotation = Quaternion.Slerp(rotInicial, rotAtras, t / (duracionGolpe * 0.5f));
            t += Time.deltaTime;
            yield return null;
        }

        t = 0f;
        while (t < duracionGolpe * 0.5f)
        {
            if (objetoActual != hacha) { puedeAtacar = true; swingCR = null; yield break; }
            hacha.transform.localPosition = Vector3.Lerp(posAtras, posAdelante, t / (duracionGolpe * 0.5f));
            hacha.transform.localRotation = Quaternion.Slerp(rotAtras, rotAdelante, t / (duracionGolpe * 0.5f));
            t += Time.deltaTime;
            yield return null;
        }

        DetectarImpacto();

        t = 0f;
        while (t < duracionVuelta)
        {
            if (objetoActual != hacha) { puedeAtacar = true; swingCR = null; yield break; }
            hacha.transform.localPosition = Vector3.Lerp(posAdelante, posInicial, t / duracionVuelta);
            hacha.transform.localRotation = Quaternion.Slerp(rotAdelante, rotInicial, t / duracionVuelta);
            t += Time.deltaTime;
            yield return null;
        }

        hacha.transform.localPosition = hachaPos0;
        hacha.transform.localRotation = hachaRot0;

        yield return new WaitForSeconds(cdAtaque);
        puedeAtacar = true;
        swingCR = null;
    }

    void DetectarImpacto()
    {
        if (cam == null) return;

        Ray rayo = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(rayo, out RaycastHit hit, rangoGolpe, capaArboles, QueryTriggerInteraction.Ignore))
        {
            var arbol = hit.collider.GetComponent<Arbol>();
            if (arbol != null)
                arbol.RecibirGolpe();
        }
    }

    // ========== PREVIEW / COLOCACIÓN ==========
    void ManejarPreviewConstruccion()
    {
        if (previewActual == null) return;
        Debug.Log("a");
        Vector3 targetPos;
        if (puntoColocacion != null) targetPos = puntoColocacion.position;
        else if (cam != null) targetPos = cam.transform.position + cam.transform.forward * distanciaPreview;
        else targetPos = transform.position + transform.forward * distanciaPreview;

        if (targetPos.y < minYPlacement) targetPos.y = minYPlacement;

        previewActual.transform.position = targetPos;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.0001f)
            previewActual.transform.Rotate(Vector3.up, scroll * 100f, Space.World);

        if (Input.GetMouseButtonDown(0))
            ColocarEstructura();
    }

    void ColocarEstructura()
    {
        if (previewActual == null || prefabSeleccionado == null) return;

        Vector3 spawnPos = previewActual.transform.position;
        if (spawnPos.y < minYPlacement) spawnPos.y = minYPlacement;

        Instantiate(prefabSeleccionado, spawnPos, previewActual.transform.rotation);

        Destroy(previewActual);
        previewActual = null;
        prefabSeleccionado = null;
    }

    public void IniciarConstruccion(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("InventarioJugador.IniciarConstruccion: prefab null");
            return;
        }

        prefabSeleccionado = prefab;

        if (previewActual != null)
            Destroy(previewActual);

        previewActual = Instantiate(prefabSeleccionado);

        foreach (var c in previewActual.GetComponentsInChildren<Collider>())
            c.enabled = false;

        foreach (var r in previewActual.GetComponentsInChildren<Renderer>())
        {
            var mats = r.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                Color baseCol = mats[i].color;
                mats[i].color = new Color(baseCol.r, baseCol.g, baseCol.b, 0.5f);
            }
        }

        if (puntoColocacion != null)
            previewActual.transform.position = puntoColocacion.position;
        else if (cam != null)
            previewActual.transform.position = cam.transform.position + cam.transform.forward * distanciaPreview;
        else
            previewActual.transform.position = transform.position + transform.forward * distanciaPreview;

        Vector3 p = previewActual.transform.position;
        if (p.y < minYPlacement) p.y = minYPlacement;
        previewActual.transform.position = p;
    }

    // ================= TRONCOS =================
    public void AgregarTronco(int cantidad)
    {
        troncos += cantidad;
        ActualizarUI();
    }

    public bool GastarTroncos(int cantidad)
    {
        if (troncos >= cantidad)
        {
            troncos -= cantidad;
            ActualizarUI();
            return true;
        }
        return false;
    }

    void ActualizarUI()
    {
        if (troncosTexto != null)
            troncosTexto.text = "Troncos: " + troncos;
    }
}
