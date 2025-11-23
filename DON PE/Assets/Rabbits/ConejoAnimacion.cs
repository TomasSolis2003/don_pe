using UnityEngine;

public class AnimacionConejo : MonoBehaviour
{
    public Animator animator;              // referencia al Animator
    public float umbralMovimiento = 0.1f;  // sensibilidad para detectar movimiento

    private Vector3 posicionAnterior;
    private bool enMovimiento = false;

    void Start()
    {
        posicionAnterior = transform.position;
    }

    void Update()
    {
        // Calcula la distancia recorrida desde el último frame
        float distancia = Vector3.Distance(transform.position, posicionAnterior);

        // Determina si el conejo se está moviendo
        if (distancia > umbralMovimiento)
            enMovimiento = true;
        else
            enMovimiento = false;

        // Actualiza parámetro del Animator
        animator.SetBool("EnMovimiento", enMovimiento);

        // Actualiza referencia para el próximo frame
        posicionAnterior = transform.position;
    }
}
