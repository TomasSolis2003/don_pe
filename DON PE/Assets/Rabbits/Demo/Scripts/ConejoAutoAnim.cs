/*using UnityEngine;

public class ConejoAutoAnim : MonoBehaviour
{
    public Animator animator;

    public float umbralMovimiento = 0.05f;

    private Vector3 posicionAnterior;
    private bool estaVivo = true;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        posicionAnterior = transform.position;
    }

    void Update()
    {
        if (!estaVivo) return;

        float distancia = Vector3.Distance(transform.position, posicionAnterior);

        if (distancia > umbralMovimiento)
        {
            // Animación de movimiento (Run)
            CambiarAnimacion(1);
        }
        else
        {
            // Animación Idle
            CambiarAnimacion(0);
        }

        posicionAnterior = transform.position;
    }

    public void Morir()
    {
        if (!estaVivo) return;

        estaVivo = false;
        CambiarAnimacion(2);  // Dead
    }

    void CambiarAnimacion(int index)
    {
        animator.SetInteger("AnimIndex", index);
        animator.SetTrigger("Next");
    }
}
*/
using UnityEngine;

public class ConejoAutoAnim : MonoBehaviour
{
    public Animator animator;

    public float velocidadMin = 0.05f;
    private Vector3 posicionAnterior;
    private bool estaVivo = true;

    private Rigidbody rb;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>(); // si no tiene, no pasa nada

        posicionAnterior = transform.position;
    }

    void Update()
    {
        if (!estaVivo) return;

        float velocidad = CalcularVelocidad();

        if (velocidad > velocidadMin)
            CambiarAnimacion(1);   // RUN
        else
            CambiarAnimacion(0);   // IDLE
    }

    float CalcularVelocidad()
    {
        if (rb != null)
            return rb.velocity.magnitude;      // si hay rigidbody → 100% exacto

        // sino, cálculo manual
        float dist = Vector3.Distance(transform.position, posicionAnterior);
        posicionAnterior = transform.position;

        return dist / Time.deltaTime;
    }

    public void Morir()
    {
        if (!estaVivo) return;
        estaVivo = false;

        CambiarAnimacion(2); // DEAD
    }

    void CambiarAnimacion(int index)
    {
        animator.SetInteger("AnimIndex", index);
        animator.SetTrigger("Next");
    }
}
