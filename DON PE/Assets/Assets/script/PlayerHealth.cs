using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDañoRecibible
{
    [Header("Vida")]
    public int vidaMax = 100;
    public int vidaActual;

    [Header("UI (opcional)")]
    public Slider barraVida;                 // Slider con Fill
    public TextMeshProUGUI textoVida;        // "80 / 100" (opcional)

    void Awake()
    {
        vidaActual = vidaMax;
        RefrescarUI();
    }

    public void RecibirDaño(int cantidad)
    {
        vidaActual = Mathf.Max(vidaActual - Mathf.Abs(cantidad), 0);
        RefrescarUI();
        if (vidaActual <= 0) Morir();
    }

    public void Curar(int cantidad)
    {
        vidaActual = Mathf.Min(vidaActual + Mathf.Abs(cantidad), vidaMax);
        RefrescarUI();
    }

    void RefrescarUI()
    {
        if (barraVida) barraVida.value = (float)vidaActual / vidaMax;
        if (textoVida) textoVida.text = $"{vidaActual} / {vidaMax}";
    }

    void Morir()
    {
        // TODO: respawn, animación, deshabilitar input, etc.
        Debug.Log("Jugador muerto");
    }
}
