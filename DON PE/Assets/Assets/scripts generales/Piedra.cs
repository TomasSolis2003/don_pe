using UnityEngine;

public class Piedra : MonoBehaviour, IDañable
{
    [Header("Resistencia de la piedra")]
    public int golpesNecesarios = 4;
    private int golpesRecibidos = 0;
    private bool destruida = false;

    [Header("Prefab de roca rota o drops (opcional)")]
    public GameObject prefabRocaRota;
    public GameObject dropMineral; // puede ser un objeto recolectable
    public SueloFertil sueloFertil;

    public void RecibirDaño(int cantidad)
    {
        if (destruida) return;

        golpesRecibidos += cantidad;

        // efecto visual opcional (pequeña vibración o sonido)
        // GetComponent<Renderer>().material.color = Color.Lerp(Color.gray, Color.black, (float)golpesRecibidos / golpesNecesarios);

        if (golpesRecibidos >= golpesNecesarios)
        {
            Romper();
        }
    }

    void Romper()
    {
        destruida = true;

        // Efecto visual o sonoro
        if (prefabRocaRota != null)
            Instantiate(prefabRocaRota, transform.position, Quaternion.identity);

        if (dropMineral != null)
            Instantiate(dropMineral, transform.position + Vector3.up * 0.5f, Quaternion.identity);

        // Si usás SueloFertil, podés liberar la posición
        if (sueloFertil != null)
            sueloFertil.ArbolTalado(transform.position);

        Destroy(gameObject);
    }
}
