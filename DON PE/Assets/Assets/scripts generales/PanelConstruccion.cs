
using UnityEngine;

public class PanelConstruccion : MonoBehaviour
{
    public InventarioJugador inventario;
    public BotonEstructuraUI[] botones;

    void Start()
    {
        foreach (var b in botones)
        {
            if (b != null)
                b.Inicializar(this);
        }
    }

    public void SeleccionarEstructura(BotonEstructuraUI boton)
    {
        if (!inventario) return;

        // igual que antes: mejor gastar al colocar, pero si querés gastar acá, cambiás esto
        if (inventario.GastarTroncos(boton.costoTroncos))
        {
            inventario.IniciarConstruccion(boton.prefab);
        }
        else
        {
            Debug.Log($"No tenés troncos suficientes para {boton.nombre}");
        }
    }
}
