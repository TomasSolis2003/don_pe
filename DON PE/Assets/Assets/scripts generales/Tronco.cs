/*using UnityEngine;

public class Tronco : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        InventarioJugador inv = other.GetComponent<InventarioJugador>();
        if (inv != null)
        {
            inv.AgregarTronco(1);
            Destroy(gameObject);
        }
    }
}
*/
/*using UnityEngine;
using static UnityEditor.Progress;

public class Tronco : MonoBehaviour
{
    public Item itemTronco; // arrastra aquí el ScriptableObject "Tronco"

    void OnTriggerEnter(Collider other)
    {
        InventarioJugador inventario = other.GetComponent<InventarioJugador>();

        if (inventario != null)
        {
            inventario.Agregar(itemTronco);
            Destroy(gameObject); // desaparece el tronco del suelo
        }
    }
}
*/
using UnityEngine;
using static UnityEditor.Progress;

public class Tronco : MonoBehaviour
{
    public Item itemTronco;

    /* void OnTriggerEnter(Collider other)
     {
         InventarioJugador inventario = other.GetComponent<InventarioJugador>();

         if (inventario != null)
         {
             inventario.Agregar(itemTronco);
             Destroy(gameObject);
         }
     }*/
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
