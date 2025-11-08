

/*using System.Collections.Generic;
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
*/
/*using System.Collections.Generic;
using UnityEngine;

public class SueloFertil : MonoBehaviour
{
    // ================== NUEVO: Cantidades fijas por tipo ==================
    [Header("Cantidades fijas por tipo")]
    public int cantidadArbolNormal = 20;
    public int cantidadArbolGigante = 5;
    public int cantidadRoca = 6;
    public int cantidadHongo = 4;
    public int cantidadArbusto = 5;

    [Header("Altura permitida")]
    public float minAlturaPermitida = -2f;
    public float maxAlturaPermitida = 3f;

    [Header("Detección del terreno")]
    public LayerMask capaTerreno;

    [Header("Prefabs principales")]
    public GameObject arbolNormalPrefab;
    public GameObject arbolGigantePrefab;
    public GameObject prefabTronco;

    [Header("Prefabs adicionales")]
    public GameObject rocaPrefab;
    public GameObject arbustoMorasPrefab;
    public GameObject hongoPrefab;

    [Header("Configuración general")]
    public Vector2 limitesZona = new Vector2(50, 50);

    [Header("Distancias mínimas")]
    public float distanciaMinNormal = 5f;
    public float distanciaMinGigante = 10f;
    public float distanciaMinRoca = 4f;
    public float distanciaMinHongo = 2f;
    public float distanciaMinArbusto = 3f;

    [Header("Colisiones y capas")]
    public LayerMask capaEstructuras;

    private List<Vector3> posicionesUsadas = new List<Vector3>();
    private int recursosActuales = 0;

    [Header("Bloqueo de crecimiento por estructuras")]
    [Tooltip("Evita que crezcan árboles cerca de construcciones.")]
    public List<(Vector3 posicion, float radio)> zonasBloqueadas = new List<(Vector3, float)>();

    // ================== NUEVO: tipos internos ==================
    private enum TipoRecurso { ArbolNormal, ArbolGigante, Roca, Hongo, Arbusto }

    private struct PedidoSpawn
    {
        public TipoRecurso tipo;
        public GameObject prefab;
        public float distMin;
    }

    void Start()
    {
        GenerarRecursos();
    }

    // ---------------- GENERACIÓN ----------------

    void GenerarRecursos()
    {
        // Arma la lista de pedidos en base a las cantidades fijas
        List<PedidoSpawn> pedidos = ConstruirPedidos();
        Mezclar(pedidos); // shuffle para no agrupar por tipo

        // Intentos por pedido para evitar loops eternos si el área está muy llena
        const int intentosPorPedido = 30;

        foreach (var pedido in pedidos)
        {
            bool colocado = false;
            for (int i = 0; i < intentosPorPedido && !colocado; i++)
            {
                Vector3 pos;
                if (!ProbarPosicionValida(out pos)) continue;

                if (!EsPosicionValida(pos, pedido.distMin)) continue;

                // Instanciar
                GameObject recurso = Instantiate(pedido.prefab, pos, Quaternion.identity);
                recurso.transform.parent = transform;
                recurso.transform.localScale = Vector3.one;
                recurso.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

                posicionesUsadas.Add(pos);
                recursosActuales++;

                // Inicialización de árbol si corresponde
                if (recurso.CompareTag("Arbol"))
                {
                    Arbol scriptArbol = recurso.GetComponent<Arbol>();
                    if (scriptArbol != null)
                    {
                        bool esGigante = (pedido.tipo == TipoRecurso.ArbolGigante);
                        scriptArbol.Inicializar(this, esGigante, prefabTronco);
                    }
                }

                colocado = true;
            }
            // Si no se pudo colocar tras X intentos, se salta: el área está saturada o bloqueada
        }
    }

    // ================== NUEVO: construir pedidos con cantidades fijas ==================
    List<PedidoSpawn> ConstruirPedidos()
    {
        var pedidos = new List<PedidoSpawn>();

        void Agregar(TipoRecurso tipo, GameObject prefab, float dist, int cantidad)
        {
            if (prefab == null || cantidad <= 0) return;
            for (int i = 0; i < cantidad; i++)
                pedidos.Add(new PedidoSpawn { tipo = tipo, prefab = prefab, distMin = dist });
        }

        Agregar(TipoRecurso.ArbolNormal, arbolNormalPrefab, distanciaMinNormal, cantidadArbolNormal);
        Agregar(TipoRecurso.ArbolGigante, arbolGigantePrefab, distanciaMinGigante, cantidadArbolGigante);
        Agregar(TipoRecurso.Roca, rocaPrefab, distanciaMinRoca, cantidadRoca);
        Agregar(TipoRecurso.Hongo, hongoPrefab, distanciaMinHongo, cantidadHongo);
        Agregar(TipoRecurso.Arbusto, arbustoMorasPrefab, distanciaMinArbusto, cantidadArbusto);

        return pedidos;
    }

    // ================== NUEVO: shuffle Fisher–Yates ==================
    void Mezclar<T>(IList<T> lista)
    {
        for (int i = lista.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (lista[i], lista[j]) = (lista[j], lista[i]);
        }
    }

    // ---------------- POSICIÓN SOBRE TERRENO ----------------
    bool ProbarPosicionValida(out Vector3 posicionAjustada)
    {
        // Posición base aleatoria en el rectángulo local
        Vector3 basePos = new Vector3(
            Random.Range(-limitesZona.x / 2f, limitesZona.x / 2f),
            0f,
            Random.Range(-limitesZona.y / 2f, limitesZona.y / 2f)
        ) + transform.position;

        // Raycast hacia abajo para pegar al terreno
        var rayo = new Ray(basePos + Vector3.up * 200f, Vector3.down);
        if (Physics.Raycast(rayo, out RaycastHit hit, 500f, capaTerreno))
        {
            var p = hit.point;

            // Chequear altura
            if (p.y < minAlturaPermitida || p.y > maxAlturaPermitida)
            {
                posicionAjustada = Vector3.zero;
                return false;
            }

            posicionAjustada = p;
            return true;
        }

        posicionAjustada = Vector3.zero;
        return false;
    }

    // ---------------- VALIDACIÓN ----------------
    bool EsPosicionValida(Vector3 nuevaPos, float distanciaMinima)
    {
        // Distancia mínima entre recursos ya colocados
        foreach (Vector3 posExistente in posicionesUsadas)
        {
            if (Vector3.Distance(nuevaPos, posExistente) < distanciaMinima)
                return false;
        }

        // Evitar estructuras
        if (Physics.CheckSphere(nuevaPos, 2f, capaEstructuras))
            return false;

        // Zonas bloqueadas
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
                    Destroy(h.gameObject);
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
            Gizmos.DrawSphere(z.posicion, z.radio);

        Gizmos.color = Color.yellow;
        foreach (var pos in posicionesUsadas)
            Gizmos.DrawSphere(pos + Vector3.up * 0.5f, 0.3f);
    }

    // ---------------- REGENERACIÓN DE ÁRBOLES ----------------
    public void ArbolTalado(Vector3 posicionArbol)
    {
        posicionesUsadas.Remove(posicionArbol);
        recursosActuales--;

        // Reforestación: volvemos a intentar spawnear UN ÁRBOL NORMAL (o el que definas)
        StartCoroutine(ReforestarTrasTiempo());
    }

    private System.Collections.IEnumerator ReforestarTrasTiempo()
    {
        // Podés variar este rango o hacerlo depender de “fertilidad”
        float tiempo = Random.Range(60f, 120f);
        yield return new WaitForSeconds(tiempo);

        // Intentá solo un pedido de árbol normal (o adapta a tu lógica)
        var pedido = new PedidoSpawn
        {
            tipo = TipoRecurso.ArbolNormal,
            prefab = arbolNormalPrefab,
            distMin = distanciaMinNormal
        };

        const int intentos = 30;
        for (int i = 0; i < intentos; i++)
        {
            Vector3 pos;
            if (!ProbarPosicionValida(out pos)) continue;
            if (!EsPosicionValida(pos, pedido.distMin)) continue;

            GameObject recurso = Instantiate(pedido.prefab, pos, Quaternion.identity, transform);
            recurso.transform.localScale = Vector3.one;
            recurso.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

            posicionesUsadas.Add(pos);
            recursosActuales++;

            if (recurso.CompareTag("Arbol"))
            {
                var scriptArbol = recurso.GetComponent<Arbol>();
                if (scriptArbol != null)
                    scriptArbol.Inicializar(this, false, prefabTronco);
            }
            break;
        }
    }
}
*/
using System.Collections.Generic;
using UnityEngine;

public class SueloFertil : MonoBehaviour
{
    [Header("Cantidades fijas por tipo")]
    public int cantidadArbolNormal = 20;
    public int cantidadArbolGigante = 5;
    public int cantidadRoca = 6;
    public int cantidadHongo = 4;
    public int cantidadArbusto = 5;

    [Header("Altura permitida")]
    public float minAlturaPermitida = -2f;
    public float maxAlturaPermitida = 3f;

    [Header("Detección del terreno")]
    public LayerMask capaTerreno;

    [Header("Prefabs principales")]
    public GameObject arbolNormalPrefab;
    public GameObject arbolGigantePrefab;
    public GameObject prefabTronco;

    [Header("Prefabs adicionales")]
    public GameObject rocaPrefab;
    public GameObject arbustoMorasPrefab;
    public GameObject hongoPrefab;

    [Header("Configuración general")]
    public Vector2 limitesZona = new Vector2(50, 50);

    [Header("Distancias mínimas")]
    public float distanciaMinNormal = 5f;
    public float distanciaMinGigante = 10f;
    public float distanciaMinRoca = 4f;
    public float distanciaMinHongo = 2f;
    public float distanciaMinArbusto = 3f;

    [Header("Colisiones y capas")]
    public LayerMask capaEstructuras;

    private List<Vector3> posicionesUsadas = new List<Vector3>();
    private int recursosActuales = 0;

    [Header("Bloqueo de crecimiento por estructuras")]
    [Tooltip("Evita que crezcan árboles cerca de construcciones.")]
    public List<(Vector3 posicion, float radio)> zonasBloqueadas = new List<(Vector3, float)>();

    private enum TipoRecurso { ArbolNormal, ArbolGigante, Roca, Hongo, Arbusto }

    private struct PedidoSpawn
    {
        public TipoRecurso tipo;
        public GameObject prefab;
        public float distMin;
    }

    void Start()
    {
        GenerarRecursos();
    }

    void GenerarRecursos()
    {
        List<PedidoSpawn> pedidos = ConstruirPedidos();
        Mezclar(pedidos);

        const int intentosPorPedido = 30;

        foreach (var pedido in pedidos)
        {
            bool colocado = false;
            for (int i = 0; i < intentosPorPedido && !colocado; i++)
            {
                Vector3 pos;
                if (!ProbarPosicionValida(out pos)) continue;
                if (!EsPosicionValida(pos, pedido.distMin)) continue;

                GameObject recurso = Instantiate(pedido.prefab, pos, Quaternion.identity);
                recurso.transform.parent = transform;
                recurso.transform.localScale = Vector3.one;
                recurso.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

                posicionesUsadas.Add(pos);
                recursosActuales++;

                // 🔧 Corrección: se eliminó "Inicializar()" y se reemplazó por asignaciones directas
                if (recurso.CompareTag("Arbol"))
                {
                    Arbol scriptArbol = recurso.GetComponent<Arbol>();
                    if (scriptArbol != null)
                    {
                        scriptArbol.sueloFertil = this;
                        scriptArbol.prefabTronco = prefabTronco;
                    }
                }

                colocado = true;
            }
        }
    }

    List<PedidoSpawn> ConstruirPedidos()
    {
        var pedidos = new List<PedidoSpawn>();

        void Agregar(TipoRecurso tipo, GameObject prefab, float dist, int cantidad)
        {
            if (prefab == null || cantidad <= 0) return;
            for (int i = 0; i < cantidad; i++)
                pedidos.Add(new PedidoSpawn { tipo = tipo, prefab = prefab, distMin = dist });
        }

        Agregar(TipoRecurso.ArbolNormal, arbolNormalPrefab, distanciaMinNormal, cantidadArbolNormal);
        Agregar(TipoRecurso.ArbolGigante, arbolGigantePrefab, distanciaMinGigante, cantidadArbolGigante);
        Agregar(TipoRecurso.Roca, rocaPrefab, distanciaMinRoca, cantidadRoca);
        Agregar(TipoRecurso.Hongo, hongoPrefab, distanciaMinHongo, cantidadHongo);
        Agregar(TipoRecurso.Arbusto, arbustoMorasPrefab, distanciaMinArbusto, cantidadArbusto);

        return pedidos;
    }

    void Mezclar<T>(IList<T> lista)
    {
        for (int i = lista.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (lista[i], lista[j]) = (lista[j], lista[i]);
        }
    }

    bool ProbarPosicionValida(out Vector3 posicionAjustada)
    {
        Vector3 basePos = new Vector3(
            Random.Range(-limitesZona.x / 2f, limitesZona.x / 2f),
            0f,
            Random.Range(-limitesZona.y / 2f, limitesZona.y / 2f)
        ) + transform.position;

        var rayo = new Ray(basePos + Vector3.up * 200f, Vector3.down);
        if (Physics.Raycast(rayo, out RaycastHit hit, 500f, capaTerreno))
        {
            var p = hit.point;
            if (p.y < minAlturaPermitida || p.y > maxAlturaPermitida)
            {
                posicionAjustada = Vector3.zero;
                return false;
            }

            posicionAjustada = p;
            return true;
        }

        posicionAjustada = Vector3.zero;
        return false;
    }

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
                    Destroy(h.gameObject);
            }
        }

        posicionesUsadas = nuevasPosiciones;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(limitesZona.x, 0.1f, limitesZona.y));

        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        foreach (var z in zonasBloqueadas)
            Gizmos.DrawSphere(z.posicion, z.radio);

        Gizmos.color = Color.yellow;
        foreach (var pos in posicionesUsadas)
            Gizmos.DrawSphere(pos + Vector3.up * 0.5f, 0.3f);
    }

    public void ArbolTalado(Vector3 posicionArbol)
    {
        posicionesUsadas.Remove(posicionArbol);
        recursosActuales--;
        StartCoroutine(ReforestarTrasTiempo());
    }

    private System.Collections.IEnumerator ReforestarTrasTiempo()
    {
        float tiempo = Random.Range(60f, 120f);
        yield return new WaitForSeconds(tiempo);

        var pedido = new PedidoSpawn
        {
            tipo = TipoRecurso.ArbolNormal,
            prefab = arbolNormalPrefab,
            distMin = distanciaMinNormal
        };

        const int intentos = 30;
        for (int i = 0; i < intentos; i++)
        {
            Vector3 pos;
            if (!ProbarPosicionValida(out pos)) continue;
            if (!EsPosicionValida(pos, pedido.distMin)) continue;

            GameObject recurso = Instantiate(pedido.prefab, pos, Quaternion.identity, transform);
            recurso.transform.localScale = Vector3.one;
            recurso.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

            posicionesUsadas.Add(pos);
            recursosActuales++;

            if (recurso.CompareTag("Arbol"))
            {
                var scriptArbol = recurso.GetComponent<Arbol>();
                if (scriptArbol != null)
                {
                    scriptArbol.sueloFertil = this;
                    scriptArbol.prefabTronco = prefabTronco;
                }
            }
            break;
        }
    }
}
