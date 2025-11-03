/*using UnityEngine;
using System.Collections;

public class Hacha : MonoBehaviour
{
    [Header("Configuración del ataque")]
    public float tiempoBajada = 0.2f;
    public float tiempoSubida = 0.4f;
    public float cooldown = 0.6f; // tiempo total antes del próximo golpe

    private bool puedeAtacar = true;
    private Quaternion rotacionInicial;

    void Start()
    {
        rotacionInicial = transform.localRotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && puedeAtacar) // click izquierdo
        {
            StartCoroutine(AnimarAtaque());
        }
    }

    IEnumerator AnimarAtaque()
    {
        puedeAtacar = false;

        // --- BAJADA ---
        float t = 0f;
        while (t < tiempoBajada)
        {
            t += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(
                rotacionInicial,
                Quaternion.Euler(rotacionInicial.eulerAngles + new Vector3(60, 0, 0)), // Baja 60 grados
                t / tiempoBajada
            );
            yield return null;
        }

        // --- SUBIDA ---
        t = 0f;
        while (t < tiempoSubida)
        {
            t += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(
                Quaternion.Euler(rotacionInicial.eulerAngles + new Vector3(60, 0, 0)),
                rotacionInicial,
                t / tiempoSubida
            );
            yield return null;
        }

        // Reset final
        transform.localRotation = rotacionInicial;

        // Espera cooldown
        yield return new WaitForSeconds(cooldown - (tiempoBajada + tiempoSubida));

        puedeAtacar = true;
    }
}
*/
/*using UnityEngine;
using System.Collections;

public class Hacha : MonoBehaviour
{
    [Header("Configuración del ataque")]
    public float tiempoBajada = 0.2f;
    public float tiempoSubida = 0.4f;
    public float cooldown = 0.6f;

    [Header("Detección de impacto")]
    public float rangoGolpe = 2f;
    public LayerMask capasGolpeables; // asigná acá “Arboles, Arbustos, Recursos”

    private bool puedeAtacar = true;
    private Quaternion rotacionInicial;
    private Transform camaraJugador;

    void Start()
    {
        rotacionInicial = transform.localRotation;
        camaraJugador = Camera.main.transform; // para dirección del golpe
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && puedeAtacar)
        {
            StartCoroutine(AnimarAtaque());
        }
    }

    IEnumerator AnimarAtaque()
    {
        puedeAtacar = false;

        // --- BAJADA ---
        float t = 0f;
        bool golpeRealizado = false;

        while (t < tiempoBajada)
        {
            t += Time.deltaTime;

            // Rotación visual
            transform.localRotation = Quaternion.Lerp(
                rotacionInicial,
                Quaternion.Euler(rotacionInicial.eulerAngles + new Vector3(60, 0, 0)),
                t / tiempoBajada
            );

            // En el punto medio del swing, ejecutamos el golpe
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
                Quaternion.Euler(rotacionInicial.eulerAngles + new Vector3(60, 0, 0)),
                rotacionInicial,
                t / tiempoSubida
            );
            yield return null;
        }

        transform.localRotation = rotacionInicial;

        // Cooldown restante
        float restante = cooldown - (tiempoBajada + tiempoSubida);
        if (restante > 0)
            yield return new WaitForSeconds(restante);

        puedeAtacar = true;
    }

    void EjecutarGolpe()
    {
        // Desde el centro de la cámara (para precisión al apuntar)
        if (Physics.Raycast(camaraJugador.position, camaraJugador.forward, out RaycastHit hit, rangoGolpe, capasGolpeables))
        {
            Debug.Log("Golpeó: " + hit.collider.name);

            // Árboles (si usás tu script Arbol.cs)
              if (hit.collider.TryGetComponent(out Arbol arbol))
              {
                  arbol.RecibirGolpeHacha(); // o el método que uses para talar
              }
           


            // Arbustos de moras
            else if (hit.collider.TryGetComponent(out ArbustoMoras arbusto))
            {
                arbusto.RecibirGolpeHacha();
            }

            // Otros objetos destructibles
            else if (hit.collider.TryGetComponent(out IDañable dañable))
            {
                dañable.RecibirDaño(1); // interfaz opcional si querés generalizar
            }
        }
    }
}
*/
using UnityEngine;
using System.Collections;

public class Hacha : MonoBehaviour
{
    [Header("Configuración del ataque")]
    public float tiempoBajada = 0.2f;
    public float tiempoSubida = 0.4f;
    public float cooldown = 0.6f;

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

        // --- BAJADA ---
        float t = 0f;
        bool golpeRealizado = false;

        while (t < tiempoBajada)
        {
            t += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(
                rotacionInicial,
                Quaternion.Euler(rotacionInicial.eulerAngles + new Vector3(60, 0, 0)),
                t / tiempoBajada
            );

            // En la mitad de la bajada, ejecuta el golpe
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
                Quaternion.Euler(rotacionInicial.eulerAngles + new Vector3(60, 0, 0)),
                rotacionInicial,
                t / tiempoSubida
            );
            yield return null;
        }

        transform.localRotation = rotacionInicial;

        // --- COOLDOWN ---
        float restante = cooldown - (tiempoBajada + tiempoSubida);
        if (restante > 0) yield return new WaitForSeconds(restante);

        puedeAtacar = true;
    }

    void EjecutarGolpe()
    {
        if (Physics.Raycast(camaraJugador.position, camaraJugador.forward, out RaycastHit hit, rangoGolpe, capasGolpeables))
        {
            Debug.Log("Golpeó: " + hit.collider.name);

            // ✅ golpea cualquier cosa que implemente la interfaz IDañable
            if (hit.collider.TryGetComponent(out IDañable objeto))
            {
                objeto.RecibirDaño(1);
            }
        }
    }
}
