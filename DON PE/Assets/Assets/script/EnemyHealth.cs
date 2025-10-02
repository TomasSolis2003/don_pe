using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDañoRecibible
{
    public int vidaMax = 60;
    public int vidaActual;
    public bool destruirAlMorir = true;
    public GameObject efectoMuerte; // opcional

    void Awake()
    {
        vidaActual = vidaMax;
    }

    public void RecibirDaño(int cantidad)
    {
        vidaActual = Mathf.Max(vidaActual - Mathf.Abs(cantidad), 0);
        if (vidaActual <= 0) Morir();
    }

    void Morir()
    {
        if (efectoMuerte) Instantiate(efectoMuerte, transform.position, Quaternion.identity);
        if (destruirAlMorir) Destroy(gameObject);
        else enabled = false;
    }
}
