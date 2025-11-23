
/*using UnityEngine;
using System.Collections;

public class AnimalIA : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador;
    public GameObject itemDrop;
    public float vida = 30f;

    [Header("Movimiento")]
    public float velocidadCaminata = 2f;
    public float velocidadHuida = 6f;
    public float radioMerodeo = 10f;
    public float distanciaHuida = 8f;

    [Header("Pausas")]
    public float tiempoPausa = 3f;

    private Vector3 puntoDestino;
    private bool huyendo = false;
    private bool enPausa = false;

    void Start()
    {
        GenerarNuevoPunto();
    }

    void Update()
    {
        float distanciaJugador = Vector3.Distance(transform.position, jugador.position);

        // Si el jugador está cerca → huir sin importar pausas
        if (distanciaJugador < distanciaHuida)
        {
            huyendo = true;
            enPausa = false; // cancela la pausa
            HuirDelJugador();
            return;
        }

        huyendo = false;

        // Si estamos en pausa → quedarse quieto
        if (enPausa)
            return;

        Merodear();
    }

    void Merodear()
    {
        if (Vector3.Distance(transform.position, puntoDestino) < 1f)
        {
            StartCoroutine(PausaAntesDeMover());
            return;
        }

        Vector3 direccion = (puntoDestino - transform.position).normalized;
        transform.position += direccion * velocidadCaminata * Time.deltaTime;
        transform.forward = direccion;
    }

    IEnumerator PausaAntesDeMover()
    {
        enPausa = true;
        yield return new WaitForSeconds(tiempoPausa);

        // Antes de salir de la pausa, chequea si el jugador apareció cerca
        if (Vector3.Distance(transform.position, jugador.position) < distanciaHuida)
        {
            enPausa = false;
            yield break; // huir lo resolverá el Update
        }

        GenerarNuevoPunto();
        enPausa = false;
    }

    void HuirDelJugador()
    {
        Vector3 direccionHuida = (transform.position - jugador.position).normalized;
        transform.position += direccionHuida * velocidadHuida * Time.deltaTime;
        transform.forward = direccionHuida;
    }

    void GenerarNuevoPunto()
    {
        Vector2 random = Random.insideUnitCircle * radioMerodeo;
        puntoDestino = new Vector3(transform.position.x + random.x, transform.position.y, transform.position.z + random.y);
    }

    // ------ DAÑO ------
    public void RecibirDaño(float cantidad)
    {
        vida -= cantidad;

        if (vida <= 0)
            Morir();
    }

    void Morir()
    {
        if (itemDrop != null)
            Instantiate(itemDrop, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
*/
using UnityEngine;
using System.Collections;

public class AnimalIA : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador;
    public GameObject itemDrop;
    public float vida = 30f;

    [Header("Movimiento")]
    public float velocidadCaminata = 2f;
    public float velocidadHuida = 6f;
    public float radioMerodeo = 10f;
    public float distanciaHuida = 8f;

    [Header("Pausas")]
    public float tiempoPausa = 3f;

    private Vector3 puntoDestino;
    private bool huyendo = false;
    private bool enPausa = false;

    void Start()
    {
        // --- Buscamos el jugador automáticamente por TAG ---
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
            jugador = obj.transform;

        GenerarNuevoPunto();
    }

    void Update()
    {
        if (jugador == null)
            return; // evita errores si aún no encuentra player

        float distanciaJugador = Vector3.Distance(transform.position, jugador.position);

        if (distanciaJugador < distanciaHuida)
        {
            huyendo = true;
            enPausa = false;
            HuirDelJugador();
            return;
        }

        huyendo = false;

        if (enPausa)
            return;

        Merodear();
    }

    void Merodear()
    {
        if (Vector3.Distance(transform.position, puntoDestino) < 1f)
        {
            StartCoroutine(PausaAntesDeMover());
            return;
        }

        Vector3 direccion = (puntoDestino - transform.position).normalized;
        transform.position += direccion * velocidadCaminata * Time.deltaTime;
        transform.forward = direccion;
    }

    IEnumerator PausaAntesDeMover()
    {
        enPausa = true;
        yield return new WaitForSeconds(tiempoPausa);

        if (jugador != null && Vector3.Distance(transform.position, jugador.position) < distanciaHuida)
        {
            enPausa = false;
            yield break;
        }

        GenerarNuevoPunto();
        enPausa = false;
    }

    void HuirDelJugador()
    {
        Vector3 direccionHuida = (transform.position - jugador.position).normalized;
        transform.position += direccionHuida * velocidadHuida * Time.deltaTime;
        transform.forward = direccionHuida;
    }

    void GenerarNuevoPunto()
    {
        Vector2 random = Random.insideUnitCircle * radioMerodeo;
        puntoDestino = new Vector3(transform.position.x + random.x, transform.position.y, transform.position.z + random.y);
    }

    public void RecibirDaño(float cantidad)
    {
        vida -= cantidad;

        if (vida <= 0)
            Morir();
    }

    void Morir()
    {
        if (itemDrop != null)
            Instantiate(itemDrop, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
