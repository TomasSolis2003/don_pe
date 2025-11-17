using UnityEngine;
using System.Collections;

public class Pico : MonoBehaviour
{
    [Header("Configuración del ataque")]
    public float tiempoBajada = 0.25f;
    public float tiempoSubida = 0.45f;
    public float cooldown = 0.7f;

    [Header("Detección de impacto")]
    public float rangoGolpe = 2f;
    public LayerMask capasGolpeables;

    private bool puedeAtacar = true;
    private Quaternion rotacionInicial;
    private Transform camaraJugador;

    void Start()
    {
        rotacionInicial = transform.localRotation;
        camaraJugador = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && puedeAtacar)
            StartCoroutine(AnimarAtaque());
    }

    IEnumerator AnimarAtaque()
    {
        puedeAtacar = false;

        float t = 0f;
        bool golpeRealizado = false;

        // --- BAJADA ---
        while (t < tiempoBajada)
        {
            t += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(
                rotacionInicial,
                Quaternion.Euler(rotacionInicial.eulerAngles + new Vector3(70, 0, 0)), // más marcado
                t / tiempoBajada
            );

            if (!golpeRealizado && t >= tiempoBajada * 0.5f)
            {
                EjecutarGolpe();
                golpeRealizado = true;
            }

            yield return null;
        }

        // --- SUBIDA ---
        t = 0f;
        while (t < tiempoSubida)
        {
            t += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(
                Quaternion.Euler(rotacionInicial.eulerAngles + new Vector3(70, 0, 0)),
                rotacionInicial,
                t / tiempoSubida
            );
            yield return null;
        }

        transform.localRotation = rotacionInicial;

        float restante = cooldown - (tiempoBajada + tiempoSubida);
        if (restante > 0) yield return new WaitForSeconds(restante);

        puedeAtacar = true;
    }

    void EjecutarGolpe()
    {
        if (Physics.Raycast(camaraJugador.position, camaraJugador.forward, out RaycastHit hit, rangoGolpe, capasGolpeables))
        {
            Debug.Log("Golpeó con pico: " + hit.collider.name);

            if (hit.collider.TryGetComponent(out IDañable dañable))
                dañable.RecibirDaño(1);
        }
    }
}
