
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
*/

using System.Collections.Generic;
using UnityEngine;

public class SueloFertil : MonoBehaviour
{

    //separado
    [Header("Altura permitida")]
    public float minAlturaPermitida = -2f;
    public float maxAlturaPermitida = 3f;

    [Header("Detección del terreno")]
    public LayerMask capaTerreno;
//...............
    [Header("Prefabs principales")]
    public GameObject arbolNormalPrefab;
    public GameObject arbolGigantePrefab;
    public GameObject prefabTronco;

    [Header("Prefabs adicionales")]
    public GameObject rocaPrefab;
    public GameObject arbustoMorasPrefab;
    public GameObject hongoPrefab;

    [Header("Configuración general")]
    public int cantidadDeRecursos = 40;
    public Vector2 limitesZona = new Vector2(50, 50);

    [Header("Distancias mínimas")]
    public float distanciaMinNormal = 5f;
    public float distanciaMinGigante = 10f;
    public float distanciaMinRoca = 4f;
    public float distanciaMinHongo = 2f;
    public float distanciaMinArbusto = 3f;

    [Header("Probabilidades de aparición")]
    [Range(0f, 1f)] public float probabilidadArbolGigante = 0.2f;
    [Range(0f, 1f)] public float probabilidadRoca = 0.15f;
    [Range(0f, 1f)] public float probabilidadHongo = 0.15f;
    [Range(0f, 1f)] public float probabilidadArbusto = 0.25f;

    [Header("Colisiones y capas")]
    public LayerMask capaEstructuras;
   // public LayerMask capaTerreno;

    private List<Vector3> posicionesUsadas = new List<Vector3>();
    private int recursosActuales = 0;

    [Header("Bloqueo de crecimiento por estructuras")]
    [Tooltip("Evita que crezcan árboles cerca de construcciones.")]
    public List<(Vector3 posicion, float radio)> zonasBloqueadas = new List<(Vector3, float)>();

    void Start()
    {
        GenerarRecursos();
    }

    // ---------------- GENERACIÓN ----------------
    void GenerarRecursos()
    {
        int intentosMaximos = cantidadDeRecursos * 10;
        int intentos = 0;

        while (recursosActuales < cantidadDeRecursos && intentos < intentosMaximos)
        {
            intentos++;
            CrearRecurso();
        }
    }

    /* void CrearRecurso()
     {
         GameObject prefabElegido = null;
         float distanciaMinima = 3f;

         // Probabilidades combinadas
         float rnd = Random.value;
         if (rnd < probabilidadRoca && rocaPrefab) // roca
         {
             prefabElegido = rocaPrefab;
             distanciaMinima = distanciaMinRoca;
         }
         else if (rnd < probabilidadRoca + probabilidadHongo && hongoPrefab) // hongo
         {
             prefabElegido = hongoPrefab;
             distanciaMinima = distanciaMinHongo;
         }
         else if (rnd < probabilidadRoca + probabilidadHongo + probabilidadArbusto && arbustoMorasPrefab) // arbusto
         {
             prefabElegido = arbustoMorasPrefab;
             distanciaMinima = distanciaMinArbusto;
         }
         else // árbol
         {
             bool esGigante = Random.value < probabilidadArbolGigante && arbolGigantePrefab;
             prefabElegido = esGigante ? arbolGigantePrefab : arbolNormalPrefab;
             distanciaMinima = esGigante ? distanciaMinGigante : distanciaMinNormal;
         }

         if (!prefabElegido) return;

         Vector3 nuevaPosicion = new Vector3(
             Random.Range(-limitesZona.x / 2, limitesZona.x / 2),
             0,
             Random.Range(-limitesZona.y / 2, limitesZona.y / 2)
         ) + transform.position;

         // Ajustar altura según terreno
         if (Physics.Raycast(nuevaPosicion + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 50f, capaTerreno))
         {
             nuevaPosicion = hit.point;
         }

         if (EsPosicionValida(nuevaPosicion, distanciaMinima))
         {
             GameObject recurso = Instantiate(prefabElegido, nuevaPosicion, Quaternion.identity, transform);

             // Ajustar rotación aleatoria
             recurso.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

             posicionesUsadas.Add(nuevaPosicion);
             recursosActuales++;

             // Si es árbol, inicializa su script
             if (recurso.CompareTag("Arbol"))
             {
                 Arbol scriptArbol = recurso.GetComponent<Arbol>();
                 if (scriptArbol != null)
                 {
                     bool esGigante = recurso == arbolGigantePrefab;
                     scriptArbol.Inicializar(this, esGigante, prefabTronco);
                 }
             }
         }
     }
     */
    /*void CrearRecurso()
    {
        GameObject prefabElegido = null;
        float distanciaMinima = 3f;

        // --- Probabilidades combinadas ---
        float rnd = Random.value;
        if (rnd < probabilidadRoca && rocaPrefab) // roca
        {
            prefabElegido = rocaPrefab;
            distanciaMinima = distanciaMinRoca;
        }
        else if (rnd < probabilidadRoca + probabilidadHongo && hongoPrefab) // hongo
        {
            prefabElegido = hongoPrefab;
            distanciaMinima = distanciaMinHongo;
        }
        else if (rnd < probabilidadRoca + probabilidadHongo + probabilidadArbusto && arbustoMorasPrefab) // arbusto
        {
            prefabElegido = arbustoMorasPrefab;
            distanciaMinima = distanciaMinArbusto;
        }
        else // árbol
        {
            bool esGigante = Random.value < probabilidadArbolGigante && arbolGigantePrefab;
            prefabElegido = esGigante ? arbolGigantePrefab : arbolNormalPrefab;
            distanciaMinima = esGigante ? distanciaMinGigante : distanciaMinNormal;
        }

        if (!prefabElegido) return;

        // --- Generar posición inicial ---
        Vector3 nuevaPosicion = new Vector3(
            Random.Range(-limitesZona.x / 2, limitesZona.x / 2),
            0,
            Random.Range(-limitesZona.y / 2, limitesZona.y / 2)
        ) + transform.position;

        // --- Ajustar al terreno con raycast ---
        if (Physics.Raycast(nuevaPosicion + Vector3.up * 50f, Vector3.down, out RaycastHit hit, 100f, capaTerreno))
        {
            nuevaPosicion = hit.point;

            // 🔹 Límite de altura Y
            if (nuevaPosicion.y < -2f || nuevaPosicion.y > 3f) // ajustá estos valores según tu mapa
                return;
        }
        else
        {
            return; // no hay terreno debajo, no instanciar
        }

        // --- Verificar si la posición es válida ---
        if (EsPosicionValida(nuevaPosicion, distanciaMinima))
        {
            GameObject recurso = Instantiate(prefabElegido, nuevaPosicion, Quaternion.identity, transform);

            // 🔹 Aseguramos escala original
            recurso.transform.localScale = Vector3.one;

            // 🔹 Rotación aleatoria para variedad
            recurso.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

            posicionesUsadas.Add(nuevaPosicion);
            recursosActuales++;

            // Si es árbol, inicializa su script
            if (recurso.CompareTag("Arbol"))
            {
                Arbol scriptArbol = recurso.GetComponent<Arbol>();
                if (scriptArbol != null)
                {
                    bool esGigante = recurso.name.Contains("Gigante");
                    scriptArbol.Inicializar(this, esGigante, prefabTronco);
                }
            }
        }
    }*/
    void CrearRecurso()
    {
        GameObject prefabElegido = null;
        float distanciaMinima = 3f;

        // --- Probabilidades combinadas ---
        float rnd = Random.value;
        if (rnd < probabilidadRoca && rocaPrefab) // roca
        {
            prefabElegido = rocaPrefab;
            distanciaMinima = distanciaMinRoca;
        }
        else if (rnd < probabilidadRoca + probabilidadHongo && hongoPrefab) // hongo
        {
            prefabElegido = hongoPrefab;
            distanciaMinima = distanciaMinHongo;
        }
        else if (rnd < probabilidadRoca + probabilidadHongo + probabilidadArbusto && arbustoMorasPrefab) // arbusto
        {
            prefabElegido = arbustoMorasPrefab;
            distanciaMinima = distanciaMinArbusto;
        }
        else // árbol
        {
            bool esGigante = Random.value < probabilidadArbolGigante && arbolGigantePrefab;
            prefabElegido = esGigante ? arbolGigantePrefab : arbolNormalPrefab;
            distanciaMinima = esGigante ? distanciaMinGigante : distanciaMinNormal;
        }

        if (!prefabElegido) return;

        // --- Generar posición base ---
        Vector3 nuevaPosicion = new Vector3(
            Random.Range(-limitesZona.x / 2, limitesZona.x / 2),
            0,
            Random.Range(-limitesZona.y / 2, limitesZona.y / 2)
        ) + transform.position;

        // --- Ajustar al terreno ---
        Ray rayo = new Ray(nuevaPosicion + Vector3.up * 200f, Vector3.down);
        if (Physics.Raycast(rayo, out RaycastHit hit, 500f, capaTerreno))
        {
            nuevaPosicion = hit.point;

            // Limitar por altura
            if (nuevaPosicion.y < minAlturaPermitida || nuevaPosicion.y > maxAlturaPermitida)
                return;
        }
        else
        {
            return; // No se detectó suelo
        }

        // --- Verificar posición ---
        if (EsPosicionValida(nuevaPosicion, distanciaMinima))
        {
            // Instanciar con escala global y rotación aleatoria
            GameObject recurso = Instantiate(prefabElegido, nuevaPosicion, Quaternion.identity);
            recurso.transform.parent = transform;
            recurso.transform.localScale = Vector3.one;
            recurso.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

            posicionesUsadas.Add(nuevaPosicion);
            recursosActuales++;

            // Si es árbol, inicializar su script
            if (recurso.CompareTag("Arbol"))
            {
                Arbol scriptArbol = recurso.GetComponent<Arbol>();
                if (scriptArbol != null)
                {
                    bool esGigante = recurso.name.Contains("Gigante");
                    scriptArbol.Inicializar(this, esGigante, prefabTronco);
                }
            }
        }
    }

    // ---------------- VALIDACIÓN ----------------
    bool EsPosicionValida(Vector3 nuevaPos, float distanciaMinima)
    {
        foreach (Vector3 posExistente in posicionesUsadas)
        {
            if (Vector3.Distance(nuevaPos, posExistente) < distanciaMinima)
                return false;
        }

        if (Physics.CheckSphere(nuevaPos, 2f, capaEstructuras))
            return false;

        foreach (var z in zonasBloqueadas)
        {
            if (Vector3.Distance(nuevaPos, z.posicion) < z.radio)
                return false;
        }

        return true;
    }

    // ---------------- ESTRUCTURAS ----------------
    public void RegistrarEstructura(Vector3 posicion, float radioBloqueo = 6f)
    {
        zonasBloqueadas.Add((posicion, radioBloqueo));
        LimpiarRecursosCercanos(posicion, radioBloqueo);
    }

    private void LimpiarRecursosCercanos(Vector3 posicion, float radio)
    {
        List<Vector3> nuevasPosiciones = new List<Vector3>();

        foreach (var pos in posicionesUsadas)
        {
            if (Vector3.Distance(pos, posicion) > radio)
                nuevasPosiciones.Add(pos);
            else
            {
                Collider[] hits = Physics.OverlapSphere(pos, 1f);
                foreach (var h in hits)
                {
                    Destroy(h.gameObject);
                }
            }
        }

        posicionesUsadas = nuevasPosiciones;
    }

    // ---------------- GIZMOS ----------------
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(limitesZona.x, 0.1f, limitesZona.y));

        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        foreach (var z in zonasBloqueadas)
        {
            Gizmos.DrawSphere(z.posicion, z.radio);
        }

        Gizmos.color = Color.yellow;
        foreach (var pos in posicionesUsadas)
        {
            Gizmos.DrawSphere(pos + Vector3.up * 0.5f, 0.3f);
        }
    }
    // ---------------- REGENERACIÓN DE ÁRBOLES ----------------
    public void ArbolTalado(Vector3 posicionArbol)
    {
        // eliminar la posición del árbol talado
        posicionesUsadas.Remove(posicionArbol);
        recursosActuales--;

        // genera un nuevo árbol después de cierto tiempo
        float tiempo = Random.Range(60f, 120f);
        Invoke(nameof(CrearRecurso), tiempo);
    }
}
