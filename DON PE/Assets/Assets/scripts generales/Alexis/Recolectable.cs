using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Recolectable : MonoBehaviour
{
    public string Item;
    public int cantidad;

    void OnTriggerEnter(Collider other)
    {
        InvYcons inventario = other.GetComponent<InvYcons>();
        if (inventario != null)
        {
            inventario.Almacenar(Item, cantidad);
            Destroy(gameObject);
        }
    }
}
