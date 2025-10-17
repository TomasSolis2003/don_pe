using UnityEngine;

public class TrigoVida : MonoBehaviour, IDañoRecibible
{
    public int vida = 20;

    public void RecibirDaño(int cantidad)
    {
        vida -= cantidad;
        if (vida <= 0)
        {
            DestruirTrigo();
        }
    }

    void DestruirTrigo()
    {
        // acá podés agregar una animación, sonido o partícula
        Destroy(gameObject);
    }
}
