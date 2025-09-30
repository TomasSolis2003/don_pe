using UnityEngine;

public class JugadorHacha : MonoBehaviour
{
    [Header("Configuraci�n del hacha")]
    public float rango = 3f; // Distancia m�xima para talar
    public LayerMask capaArbol; // Asignar "Tree" o "Default" seg�n tu prefab

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

