/*using UnityEngine;

public class Machete : MonoBehaviour
{
    public int dano = 25;
    public float rangoGolpe = 2f;
    public float radioGolpe = 0.6f;
    public LayerMask capasAtacables;
    public float tiempoEntreGolpes = 0.5f;

    private bool puedeAtacar = true;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && puedeAtacar)
        {
            StartCoroutine(Ataque());
        }
    }

    System.Collections.IEnumerator Ataque()
    {
        puedeAtacar = false;

        if (Camera.main != null)
        {
            Ray rayo = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.SphereCast(rayo, radioGolpe, out RaycastHit hit, rangoGolpe, capasAtacables, QueryTriggerInteraction.Ignore))
            {
                var objetivo = hit.collider.GetComponentInParent<IDañoRecibible>();
                if (objetivo != null) objetivo.RecibirDaño(dano);
            }
        }

        yield return new WaitForSeconds(tiempoEntreGolpes);
        puedeAtacar = true;
    }
}*/
/*using System.Collections;
using UnityEngine;

public class Machete : MonoBehaviour
{
    [Header("Ataque")]
    public int dano = 25;
    public float rangoGolpe = 2f;         // hasta dónde llega
    public float anguloGolpe = 60f;       // ancho del cono en grados
    public float radioColision = 1f;      // radio de la esfera para encontrar objetivos
    public LayerMask capasAtacables;
    public float tiempoEntreGolpes = 0.6f;

    [Header("Animación")]
    public float duracionSwing = 0.3f;    // tiempo de ida y vuelta
    public Vector3 rotacionSwing = new Vector3(0f, 60f, 0f); // izquierda -> derecha

    private bool puedeAtacar = true;
    private Vector3 pos0;
    private Quaternion rot0;

    void Start()
    {
        pos0 = transform.localPosition;
        rot0 = transform.localRotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && puedeAtacar)
            StartCoroutine(Ataque());
    }

    IEnumerator Ataque()
    {
        puedeAtacar = false;

        // --- Animación ---
        float t = 0f;
        while (t < duracionSwing)
        {
            float p = t / duracionSwing; // 0→1
            float curva = Mathf.Sin(p * Mathf.PI); // ida y vuelta
            transform.localRotation = rot0 * Quaternion.Euler(0f, -rotacionSwing.y * (1 - p) + rotacionSwing.y * p, 0f);
            t += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = rot0;

        // --- Daño en cono ---
        AtacarCono();

        yield return new WaitForSeconds(tiempoEntreGolpes);
        puedeAtacar = true;
    }

    void AtacarCono()
    {
        // Centro del cono: posición del jugador + adelante
        Vector3 origen = Camera.main.transform.position;
        Vector3 forward = Camera.main.transform.forward;

        Collider[] hits = Physics.OverlapSphere(origen + forward * (rangoGolpe * 0.5f), radioColision, capasAtacables);
        foreach (var h in hits)
        {
            Vector3 dir = (h.transform.position - origen).normalized;
            float ang = Vector3.Angle(forward, dir);
            if (ang <= anguloGolpe * 0.5f)
            {
                var dmg = h.GetComponentInParent<IDañoRecibible>();
                if (dmg != null)
                    dmg.RecibirDaño(dano);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Vector3 origen = Camera.main.transform.position;
        Vector3 forward = Camera.main.transform.forward;

        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        Gizmos.DrawWireSphere(origen + forward * (rangoGolpe * 0.5f), radioColision);
    }
}

*/
using System.Collections;
using UnityEngine;

public class Machete : MonoBehaviour
{
    [Header("Ataque")]
    public int dano = 25;
    public float rangoGolpe = 2f;          // hasta dónde llega
    public float anguloGolpe = 60f;        // ancho del cono en grados
    public float radioColision = 1f;       // radio para buscar enemigos
    public LayerMask capasAtacables;
    public float tiempoEntreGolpes = 0.6f;

    [Header("Animación")]
    public float duracionSwing = 0.3f;     // duración total del swing
    public float desplazamientoX = 10f;    // cuánto se mueve de izquierda a derecha en local X
    public float inclinacionX = 30f;       // inclinación del machete en X

    private bool puedeAtacar = true;
    private Vector3 pos0;
    private Quaternion rot0;

    void Start()
    {
        pos0 = transform.localPosition;
        rot0 = transform.localRotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && puedeAtacar)
            StartCoroutine(Ataque());
    }

    IEnumerator Ataque()
    {
        puedeAtacar = false;

        float mitad = duracionSwing / 2f;

        // --- ida: izquierda (X positivo) a centro con inclinación ---
        float t = 0f;
        while (t < mitad)
        {
            float p = t / mitad;
            transform.localPosition = Vector3.Lerp(
                pos0 + Vector3.right * desplazamientoX,
                pos0,
                p
            );
            transform.localRotation = Quaternion.Lerp(
                rot0,
                rot0 * Quaternion.Euler(inclinacionX, 0f, 0f),
                p
            );
            t += Time.deltaTime;
            yield return null;
        }

        // --- vuelta: centro a derecha (X negativo) ---
        t = 0f;
        while (t < mitad)
        {
            float p = t / mitad;
            transform.localPosition = Vector3.Lerp(
                pos0,
                pos0 + Vector3.left * desplazamientoX,
                p
            );
            transform.localRotation = Quaternion.Lerp(
                rot0 * Quaternion.Euler(inclinacionX, 0f, 0f),
                rot0,
                p
            );
            t += Time.deltaTime;
            yield return null;
        }

        // Restaurar posición/rotación exacta
        transform.localPosition = pos0;
        transform.localRotation = rot0;

        // --- aplicar daño ---
        AtacarCono();

        yield return new WaitForSeconds(tiempoEntreGolpes);
        puedeAtacar = true;
    }

    void AtacarCono()
    {
        Vector3 origen = Camera.main.transform.position;
        Vector3 forward = Camera.main.transform.forward;

        Collider[] hits = Physics.OverlapSphere(
            origen + forward * (rangoGolpe * 0.5f),
            radioColision,
            capasAtacables
        );

        foreach (var h in hits)
        {
            Vector3 dir = (h.transform.position - origen).normalized;
            float ang = Vector3.Angle(forward, dir);
            if (ang <= anguloGolpe * 0.5f)
            {
                var dmg = h.GetComponentInParent<IDañoRecibible>();
                if (dmg != null)
                    dmg.RecibirDaño(dano);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Vector3 origen = Camera.main.transform.position;
        Vector3 forward = Camera.main.transform.forward;

        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        Gizmos.DrawWireSphere(origen + forward * (rangoGolpe * 0.5f), radioColision);
    }
}
