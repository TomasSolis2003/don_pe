/*using UnityEngine;
using TMPro;

public class SymbolProximity : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador;
    public TextMeshPro textoTMP;

    [Header("Distancia de activación")]
    public float distanciaActivacion = 2f;

    private string simboloLejano = "+";
    private string simboloCercano = "--(+)--";

    void Update()
    {
        if (jugador == null || textoTMP == null) return;

        float dist = Vector3.Distance(jugador.position, transform.position);

        if (dist <= distanciaActivacion)
            textoTMP.text = simboloCercano;
        else
            textoTMP.text = simboloLejano;
    }
}
*/
/*using UnityEngine;
using TMPro;

public class SymbolProximity : MonoBehaviour
{
    [Header("Referencias")]
    public TextMeshPro textoTMP;

    [Header("Distancia de activación")]
    public float distanciaActivacion = 2f;

    private Transform jugador;

    private const string simboloLejano = "+";
    private const string simboloCercano = "--(+)--";

    void Start()
    {
        // Encontrar al jugador automáticamente usando TAG
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
            jugador = obj.transform;

        if (textoTMP == null)
            textoTMP = GetComponent<TextMeshPro>(); // fallback opcional
    }

    void Update()
    {
        if (jugador == null || textoTMP == null) return;

        float dist = Vector3.Distance(jugador.position, transform.position);

        textoTMP.text = (dist <= distanciaActivacion) ? simboloCercano : simboloLejano;
    }
}
*/
using UnityEngine;
using TMPro;

public class SymbolProximity : MonoBehaviour
{
    [Header("Texto UI")]
    public TextMeshProUGUI textoTMP;

    [Header("Distancia de activación")]
    public float distanciaActivacion = 2f;

    private Transform jugador;

    private const string simboloLejano = "-";
    private const string simboloCercano = "+";

    void Start()
    {
        // Encontrar al jugador automáticamente por TAG
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
            jugador = obj.transform;

        // Si no asignaste textoTMP, intenta encontrarlo en el mismo objeto
        if (textoTMP == null)
            textoTMP = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (jugador == null || textoTMP == null) return;

        float dist = Vector3.Distance(jugador.position, transform.position);

        textoTMP.text = (dist <= distanciaActivacion) ? simboloCercano : simboloLejano;
    }
}
