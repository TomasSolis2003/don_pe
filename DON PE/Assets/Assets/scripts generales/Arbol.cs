

/*using UnityEngine;

public class Arbol : MonoBehaviour
{
    [Header("Configuración del árbol")]
    public bool esGigante = false;
    public int troncosNormales = 3;
    public int troncosGigantes = 8;
    public int vidaNormal = 3;     // golpes para talar un árbol normal
    public int vidaGigante = 6;    // golpes para talar un árbol gigante

    public GameObject prefabTronco;

    private int vidaActual;
    private bool cortado = false;
    private SueloFertil suelo; // referencia al suelo que lo generó

    public void Inicializar(SueloFertil sueloFertil, bool gigante, GameObject troncoPrefab)
    {
        suelo = sueloFertil;
        esGigante = gigante;
        prefabTronco = troncoPrefab;
        vidaActual = esGigante ? vidaGigante : vidaNormal;
    }

    // llamado desde JugadorHacha
    public void RecibirGolpe()
    {
        if (cortado) return;

        vidaActual--;

        if (vidaActual <= 0)
        {
            Cortar();
        }
    }

    void Cortar()
    {
        cortado = true;

        int cantidad = esGigante ? troncosGigantes : troncosNormales;

        for (int i = 0; i < cantidad; i++)
        {
            Vector3 pos = transform.position + new Vector3(
                Random.Range(-1.5f, 1.5f),
                0.5f,
                Random.Range(-1.5f, 1.5f)
            );
            Instantiate(prefabTronco, pos, Quaternion.identity);
        }

        // Avisamos al suelo que este árbol fue talado
        if (suelo != null)
        {
            suelo.ArbolTalado();
        }

        // Destruir el árbol después de cortar
        Destroy(gameObject);
    }
}
*/
/*using UnityEngine;
using System.Collections;

public class Arbol : MonoBehaviour
{
    [Header("Configuración del árbol")]
    public bool esGigante = false;
    public int troncosNormales = 3;
    public int troncosGigantes = 8;
    public int vidaNormal = 3;     // golpes para talar un árbol normal
    public int vidaGigante = 6;    // golpes para talar un árbol gigante

    public GameObject prefabTronco;

    private int vidaActual;
    private bool cortado = false;
    private SueloFertil suelo; // referencia al suelo que lo generó

    private Vector3 posicionOriginal; // guardamos la posición inicial

    void Awake()
    {
        posicionOriginal = transform.position;
    }

    public void Inicializar(SueloFertil sueloFertil, bool gigante, GameObject troncoPrefab)
    {
        suelo = sueloFertil;
        esGigante = gigante;
        prefabTronco = troncoPrefab;
        vidaActual = esGigante ? vidaGigante : vidaNormal;
    }

    // llamado desde el jugador
    public void RecibirGolpe(int daño = 1)
    {
        if (cortado) return;

        vidaActual -= daño;

        // Iniciar efecto de movimiento
        StartCoroutine(MoverArbol());

        if (vidaActual <= 0)
        {
            Cortar();
        }
    }

    IEnumerator MoverArbol()
    {
        // Bajamos el árbol 1 unidad en Y
        Vector3 abajo = posicionOriginal + Vector3.down * 1f;
        float t = 0f;

        // Movimiento hacia abajo
        while (t < 0.1f)
        {
            transform.position = Vector3.Lerp(posicionOriginal, abajo, t / 0.1f);
            t += Time.deltaTime;
            yield return null;
        }

        // Pausa leve
        yield return new WaitForSeconds(0.05f);

        // Movimiento de regreso
        t = 0f;
        while (t < 0.2f)
        {
            transform.position = Vector3.Lerp(abajo, posicionOriginal, t / 0.2f);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = posicionOriginal; // aseguramos que vuelva exacto
    }

    void Cortar()
    {
        cortado = true;

        int cantidad = esGigante ? troncosGigantes : troncosNormales;

        for (int i = 0; i < cantidad; i++)
        {
            Vector3 pos = transform.position + new Vector3(
                Random.Range(-1.5f, 1.5f),
                0.5f,
                Random.Range(-1.5f, 1.5f)
            );
            Instantiate(prefabTronco, pos, Quaternion.identity);
        }

     
        Destroy(gameObject);
    }
}
*/
using UnityEngine;
using System.Collections;

public class Arbol : MonoBehaviour
{
    [Header("Configuración del árbol")]
    public bool esGigante = false;
    public int troncosNormales = 3;
    public int troncosGigantes = 8;
    public int vidaNormal = 3;     // golpes para talar un árbol normal
    public int vidaGigante = 6;    // golpes para talar un árbol gigante

    public GameObject prefabTronco;

    private int vidaActual;
    private bool cortado = false;
    private SueloFertil suelo; // referencia al suelo que lo generó
    private Vector3 posicionOriginal; // guardamos la posición inicial

    void Awake()
    {
        posicionOriginal = transform.position;

        // Si por algún motivo el prefab viene sin inicializar, lo corregimos
        if (name.ToLower().Contains("gigante"))
            esGigante = true;

        if (vidaActual <= 0)
        {
            vidaActual = esGigante ? vidaGigante : vidaNormal;
            Debug.Log($"🌲 [Awake] {name} configurado como {(esGigante ? "GIGANTE" : "Normal")} con {vidaActual} de vida inicial.");
        }
    }

    public void Inicializar(SueloFertil sueloFertil, bool gigante, GameObject troncoPrefab)
    {
        suelo = sueloFertil;
        esGigante = gigante;
        prefabTronco = troncoPrefab;

        // ⚡ Forzamos la asignación correcta de vida según tipo
        vidaActual = esGigante ? vidaGigante : vidaNormal;

        Debug.Log($"🌳 [Inicializar] Árbol {(esGigante ? "GIGANTE" : "Normal")} asignado con {vidaActual} de vida.");
    }

    // llamado desde el jugador
    public void RecibirGolpe(int daño = 1)
    {
        if (cortado) return;

        vidaActual -= daño;
        Debug.Log($"💥 {name} recibió {daño} de daño. Vida restante: {vidaActual}");

        StartCoroutine(MoverArbol());

        if (vidaActual <= 0)
            Cortar();
    }

    IEnumerator MoverArbol()
    {
        Vector3 abajo = posicionOriginal + Vector3.down * 1f;
        float t = 0f;

        while (t < 0.1f)
        {
            transform.position = Vector3.Lerp(posicionOriginal, abajo, t / 0.1f);
            t += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.05f);

        t = 0f;
        while (t < 0.2f)
        {
            transform.position = Vector3.Lerp(abajo, posicionOriginal, t / 0.2f);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = posicionOriginal;
    }

    void Cortar()
    {
        cortado = true;
        int cantidad = esGigante ? troncosGigantes : troncosNormales;

        Debug.Log($"🪓 {name} talado. Generando {cantidad} troncos.");

        for (int i = 0; i < cantidad; i++)
        {
            Vector3 pos = transform.position + new Vector3(
                Random.Range(-1.5f, 1.5f),
                0.5f,
                Random.Range(-1.5f, 1.5f)
            );
            Instantiate(prefabTronco, pos, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
