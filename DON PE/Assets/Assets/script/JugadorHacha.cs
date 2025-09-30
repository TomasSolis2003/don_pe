using UnityEngine;

public class JugadorHacha : MonoBehaviour
{
    [Header("Configuración del hacha")]
    public float rango = 3f; // Distancia máxima para talar
    public LayerMask capaArbol; // Asignar "Tree" o "Default" según tu prefab

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clic izquierdo
        {
            TalarArbol();
        }
    }

    void TalarArbol()
    {
        Ray rayo = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(rayo, out RaycastHit hit, rango, capaArbol))
        {
            Arbol arbol = hit.collider.GetComponent<Arbol>();
            if (arbol != null)
            {
                arbol.RecibirGolpe();
            }
        }
    }
}

