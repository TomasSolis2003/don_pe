using UnityEngine;
using static UnityEditor.Progress;

public class Tronco : MonoBehaviour
{
    public Item itemTronco;

    void OnTriggerEnter(Collider other)
    {
        InventarioJugador inventario = other.GetComponent<InventarioJugador>();
        if (inventario != null)
        {
            inventario.AgregarTronco(1); // ⚡️ Aquí sumamos el tronco
            Destroy(gameObject);          // El tronco desaparece del suelo
        }
    }
}
