
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

    /*    void AtacarCono()
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
        */
    void AtacarCono()
    {
        Vector3 origen = Camera.main.transform.position;
        Vector3 forward = Camera.main.transform.forward;

        Collider[] hits = Physics.OverlapSphere(
            origen + forward * (rangoGolpe * 0.5f),
            radioColision,
            capasAtacables,
            QueryTriggerInteraction.Ignore
        );

        foreach (var h in hits)
        {
            Vector3 dir = (h.transform.position - origen).normalized;
            float ang = Vector3.Angle(forward, dir);
            if (ang <= anguloGolpe * 0.5f)
            {
                var dmg = h.GetComponentInParent<IDañoRecibible>();
                if (dmg != null)
                {
                    Debug.Log($"Golpeando a {h.name} ({h.transform.root.name})");
                    dmg.RecibirDaño(dano);
                }
                else
                {
                    Debug.Log($"Collider detectado sin IDañoRecibible: {h.name}");
                }
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
