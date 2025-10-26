
using System.Collections.Generic;
using UnityEngine;

public class SueloFertil : MonoBehaviour
{
    [Header("Prefabs de árboles")]
    public GameObject arbolNormalPrefab;
    public GameObject arbolGigantePrefab;

    [Header("Prefab del tronco")]
    public GameObject prefabTronco;

    [Header("Configuración de crecimiento")]
    public int cantidadDeArboles = 20;
    public Vector2 limitesZona = new Vector2(50, 50);
    public float distanciaMinNormal = 5f;
    public float distanciaMinGigante = 10f;

    [Header("Colisiones")]
    public LayerMask capaEstructuras;

    private List<Vector3> posicionesArboles = new List<Vector3>();
    private int arbolesActuales = 0;

    void Start()
    {
        GenerarArboles();
    }

    void GenerarArboles()
    {
        int intentosMaximos = cantidadDeArboles * 10;
        int intentos = 0;

        while (arbolesActuales < cantidadDeArboles && intentos < intentosMaximos)
        {
            intentos++;
            CrearArbol();
        }
    }

    void CrearArbol()
    {
        bool esGigante = Random.value > 0.7f;
        GameObject prefabElegido = esGigante ? arbolGigantePrefab : arbolNormalPrefab;
        float distanciaMinima = esGigante ? distanciaMinGigante : distanciaMinNormal;

        Vector3 nuevaPosicion = new Vector3(
            Random.Range(-limitesZona.x / 2, limitesZona.x / 2),
            0,
            Random.Range(-limitesZona.y / 2, limitesZona.y / 2)
        ) + transform.position;

        if (EsPosicionValida(nuevaPosicion, distanciaMinima))
        {
            GameObject arbol = Instantiate(prefabElegido, nuevaPosicion, Quaternion.identity);
            arbol.transform.parent = transform;
            posicionesArboles.Add(nuevaPosicion);
            arbolesActuales++;

            Arbol scriptArbol = arbol.GetComponent<Arbol>();
            if (scriptArbol != null)
            {
                scriptArbol.Inicializar(this, esGigante, prefabTronco);
            }
        }
    }

    public void ArbolTalado(Vector3 posicionArbol)
    {
        arbolesActuales--;
        posicionesArboles.Remove(posicionArbol);

        float tiempo = Random.Range(60f, 120f);
        Invoke(nameof(CrearArbol), tiempo);
    }


    bool EsPosicionValida(Vector3 nuevaPos, float distanciaMinima)
    {
        // Evita superposición entre árboles
        foreach (Vector3 posExistente in posicionesArboles)
        {
            if (Vector3.Distance(nuevaPos, posExistente) < distanciaMinima)
                return false;
        }

        // Evita colisión con estructuras físicas (colliders)
        if (Physics.CheckSphere(nuevaPos, 2f, capaEstructuras))
            return false;

        // Evita crecimiento en zonas bloqueadas (estructuras colocadas)
        foreach (var z in zonasBloqueadas)
        {
            if (Vector3.Distance(nuevaPos, z.posicion) < z.radio)
                return false;
        }

        return true;
    }

    // 🔹 Dibujar la zona fértil en la escena
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(limitesZona.x, 0.1f, limitesZona.y));

        // opcional: dibujar posiciones actuales de árboles
        Gizmos.color = Color.yellow;
        foreach (var pos in posicionesArboles)
        {
            Gizmos.DrawSphere(pos + Vector3.up * 0.5f, 0.5f);
        }
    }
    // --- NUEVO BLOQUE PARA INTERACCIÓN CON CONSTRUCCIONES ---
    [Header("Bloqueo de crecimiento por estructuras")]
    [Tooltip("Evita que crezcan árboles cerca de construcciones.")]
    public List<(Vector3 posicion, float radio)> zonasBloqueadas = new List<(Vector3, float)>();

    /// <summary>
    /// Llama este método cuando se coloca una estructura.
    /// </summary>
    public void RegistrarEstructura(Vector3 posicion, float radioBloqueo = 6f)
    {
        zonasBloqueadas.Add((posicion, radioBloqueo));
        LimpiarArbolesCercanos(posicion, radioBloqueo);
    }

    /// <summary>
    /// Elimina árboles existentes dentro del radio bloqueado.
    /// </summary>
    private void LimpiarArbolesCercanos(Vector3 posicion, float radio)
    {
        List<Vector3> nuevasPosiciones = new List<Vector3>();

        foreach (var pos in posicionesArboles)
        {
            if (Vector3.Distance(pos, posicion) > radio)
                nuevasPosiciones.Add(pos);
            else
            {
                // Destruye los árboles dentro del área bloqueada
                Collider[] hits = Physics.OverlapSphere(pos, 1f);
                foreach (var h in hits)
                {
                    if (h.CompareTag("Arbol")) // asegurate de tener este tag
                        Destroy(h.gameObject);
                }
            }
        }

        posicionesArboles = nuevasPosiciones;
    }

    /// <summary>
    /// Dibuja las zonas bloqueadas en la escena.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // Mantiene los gizmos previos
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(limitesZona.x, 0.1f, limitesZona.y));

        // Dibuja las zonas bloqueadas
        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        foreach (var z in zonasBloqueadas)
        {
            Gizmos.DrawSphere(z.posicion, z.radio);
        }
    }

}

