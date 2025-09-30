using UnityEngine;
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
