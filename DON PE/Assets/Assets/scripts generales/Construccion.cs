
/*using UnityEngine;

public class Construir : MonoBehaviour
{
    public InventarioJugador inventario;   // Referencia al inventario del jugador
    public GameObject edificioPrefab;      // Prefab del edificio a construir
    public Transform puntoConstruccion;    // Lugar donde aparecerá el edificio
    public int costoTroncos = 10;          // Cuántos troncos cuesta

    public void ConstruirEdificio()
    {
        if (inventario.GastarTroncos(costoTroncos))
        {
            Instantiate(edificioPrefab, puntoConstruccion.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("No tienes suficientes troncos!");
        }
    }
}
*/
using UnityEngine;

public class Construccion : MonoBehaviour
{
    public InventarioJugador inventario;   // Referencia al inventario del jugador
    public GameObject edificioPrefab;      // Prefab del edificio a construir
    public Transform puntoConstruccion;    // Lugar donde aparecerá el edificio
    public int costoTroncos = 10;          // Cuántos troncos cuesta

    public void ConstruirEdificio()
    {
        if (inventario.GastarTroncos(costoTroncos))
        {
            Instantiate(edificioPrefab, puntoConstruccion.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("No tienes suficientes troncos!");
        }
    }
}
