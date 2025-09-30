
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
        foreach (Vector3 posExistente in posicionesArboles)
        {
            if (Vector3.Distance(nuevaPos, posExistente) < distanciaMinima)
            {
                return false;
            }
        }

        if (Physics.CheckSphere(nuevaPos, 2f, capaEstructuras))
        {
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
}

/*using System.Collections.Generic;
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
    public Vector2 limitesZona = new Vector2(50, 50); // Tamaño del suelo fértil en X y Z
    public float distanciaMinNormal = 5f;
    public float distanciaMinGigante = 10f;

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
        bool esGigante = Random.value > 0.7f; // 30% de probabilidad de gigante
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

            // Inicializar árbol con referencia al suelo
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

        // Liberar la posición del árbol cortado
        posicionesArboles.Remove(posicionArbol);

        // Iniciar regeneración con delay aleatorio entre 1 y 2 minutos
        float tiempo = Random.Range(60f, 120f);
        Invoke(nameof(CrearArbol), tiempo);
    }

    bool EsPosicionValida(Vector3 nuevaPos, float distanciaMinima)
    {
        foreach (Vector3 posExistente in posicionesArboles)
        {
            if (Vector3.Distance(nuevaPos, posExistente) < distanciaMinima)
            {
                return false;
            }
        }
        return true;
    }

    // 🔹 Gizmos para ver el área en la Scene
    private void OnDrawGizmos()
    {
        // Color semitransparente verde
        Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
        Gizmos.DrawCube(transform.position, new Vector3(limitesZona.x, 0.1f, limitesZona.y));

        // Contorno
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(limitesZona.x, 0.1f, limitesZona.y));
    }
}
*/