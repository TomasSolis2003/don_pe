
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbustoMoras : MonoBehaviour
{
    [Header("Prefabs y configuración de moras")]
    public GameObject prefabMora;
    public float tiempoRespawnMora = 30f;
    public int maxMorasActivas = 3;
    public List<Transform> puntosDeMoras = new List<Transform>();

    [Header("Referencia al suelo fértil (opcional)")]
    public SueloFertil sueloFertil;

    private List<GameObject> morasActuales = new List<GameObject>();
    private bool destruido = false;

    void Start()
    {
        IniciarCrecimiento();

        if (sueloFertil == null)
            sueloFertil = GetComponentInParent<SueloFertil>();
    }

    // ---------------- SISTEMA DE TALADO SIMPLE ----------------
    public void RecibirGolpeHacha()
    {
        if (destruido) return;
        destruido = true;

        // Avisar al suelo fértil para liberar la posición y regenerar más tarde
        if (sueloFertil != null)
            sueloFertil.ArbolTalado(transform.position);

        // Efecto visual o sonido (opcional)
        // Instantiate(prefabParticulas, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    // ---------------- GENERACIÓN DE MORAS ----------------
    void IniciarCrecimiento()
    {
        foreach (Transform punto in puntosDeMoras)
        {
            if (morasActuales.Count < maxMorasActivas)
                CrearMora(punto);
        }
    }

    void CrearMora(Transform punto)
    {
        if (prefabMora == null) return;

        GameObject mora = Instantiate(prefabMora, punto.position, punto.rotation, punto);
        mora.transform.localScale = Vector3.one * 0.6f;

        // Conecta con el sistema de comida
        Comida script = mora.GetComponent<Comida>();
        if (script != null)
            script.AsignarOrigen(this, punto);

        morasActuales.Add(mora);
    }

    // ---------------- RECOLECCIÓN DE MORAS ----------------
    public void NotificarMoraRecolectada(GameObject mora, Transform punto)
    {
        if (morasActuales.Contains(mora))
            morasActuales.Remove(mora);

        StartCoroutine(RespawnMora(punto));
    }

    private IEnumerator RespawnMora(Transform punto)
    {
        yield return new WaitForSeconds(tiempoRespawnMora);
        CrearMora(punto);
    }
}
*/
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbustoMoras : MonoBehaviour, IDañable
{
    [Header("Prefabs y configuración de moras")]
    public GameObject prefabMora;
    public float tiempoRespawnMora = 30f;
    public int maxMorasActivas = 3;
    public List<Transform> puntosDeMoras = new List<Transform>();

    [Header("Referencia al suelo fértil (opcional)")]
    public SueloFertil sueloFertil;

    private List<GameObject> morasActuales = new List<GameObject>();
    private bool destruido = false;

    void Start()
    {
        IniciarCrecimiento();

        if (sueloFertil == null)
            sueloFertil = GetComponentInParent<SueloFertil>();
    }

    // -------- INTERFAZ IDañable --------
    public void RecibirDaño(int cantidad)
    {
        // Un solo golpe basta
        RecibirGolpeHacha();
    }

    // -------- LÓGICA DE DESTRUCCIÓN --------
    public void RecibirGolpeHacha()
    {
        if (destruido) return;
        destruido = true;

        // Avisar al suelo fértil para regenerar más tarde
        if (sueloFertil != null)
            sueloFertil.ArbolTalado(transform.position);

        // Acá podrías poner un efecto visual o sonido
        Destroy(gameObject);
    }

    // -------- SISTEMA DE MORAS --------
    void IniciarCrecimiento()
    {
        foreach (Transform punto in puntosDeMoras)
        {
            if (morasActuales.Count < maxMorasActivas)
                CrearMora(punto);
        }
    }

    void CrearMora(Transform punto)
    {
        if (prefabMora == null) return;

        GameObject mora = Instantiate(prefabMora, punto.position, punto.rotation, punto);
        mora.transform.localScale = Vector3.one * 0.6f;

        Comida script = mora.GetComponent<Comida>();
        if (script != null)
            script.AsignarOrigen(this, punto);

        morasActuales.Add(mora);
    }

    public void NotificarMoraRecolectada(GameObject mora, Transform punto)
    {
        if (morasActuales.Contains(mora))
            morasActuales.Remove(mora);

        StartCoroutine(RespawnMora(punto));
    }

    private IEnumerator RespawnMora(Transform punto)
    {
        yield return new WaitForSeconds(tiempoRespawnMora);
        CrearMora(punto);
    }
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbustoMoras : MonoBehaviour, IDañable
{
    [Header("Prefabs y configuración de moras")]
    public GameObject prefabMora;
    public float tiempoRespawnMora = 30f;
    public int maxMorasActivas = 3;
    public List<Transform> puntosDeMoras = new List<Transform>();

    [Header("Referencia al suelo fértil (opcional)")]
    public SueloFertil sueloFertil;

    private List<GameObject> morasActuales = new List<GameObject>();
    private bool destruido = false;

    void Start()
    {
        IniciarCrecimiento();

        if (sueloFertil == null)
            sueloFertil = GetComponentInParent<SueloFertil>();
    }

    // -------- INTERFAZ IDañable --------
    public void RecibirDaño(int cantidad)
    {
        // Un solo golpe basta
        RecibirGolpeHacha();
    }

    // -------- LÓGICA DE DESTRUCCIÓN --------
    public void RecibirGolpeHacha()
    {
        if (destruido) return;
        destruido = true;

        // Avisar al suelo fértil para regenerar más tarde
        if (sueloFertil != null)
            sueloFertil.ArbolTalado(transform.position);

        // Acá podrías poner un efecto visual o sonido
        Destroy(gameObject);
    }

    // -------- SISTEMA DE MORAS --------
    void IniciarCrecimiento()
    {
        foreach (Transform punto in puntosDeMoras)
        {
            if (morasActuales.Count < maxMorasActivas)
                CrearMora(punto);
        }
    }

    void CrearMora(Transform punto)
    {
        if (prefabMora == null) return;

        GameObject mora = Instantiate(prefabMora, punto.position, punto.rotation, punto);
        mora.transform.localScale = Vector3.one * 0.6f;

        Comida script = mora.GetComponent<Comida>();
        if (script != null)
            script.AsignarOrigen(this, punto);

        morasActuales.Add(mora);
    }

    public void NotificarMoraRecolectada(GameObject mora, Transform punto)
    {
        if (morasActuales.Contains(mora))
            morasActuales.Remove(mora);

        StartCoroutine(RespawnMora(punto));
    }

    private IEnumerator RespawnMora(Transform punto)
    {
        yield return new WaitForSeconds(tiempoRespawnMora);
        CrearMora(punto);
    }
}
