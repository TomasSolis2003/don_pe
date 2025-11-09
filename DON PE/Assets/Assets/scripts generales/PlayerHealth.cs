using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDañoRecibible
{
    [Header("Vida")]
    public int vidaMax = 100;
    public int vidaActual;

    [Header("Invulnerabilidad")]
    public float tiempoInvulnerable = 1.5f;   // segundos de invulnerabilidad
    private bool esInvulnerable = false;      // bandera para no recibir daño

    [Header("UI (opcional)")]
    public Slider barraVida;
    public TextMeshProUGUI textoVida;

    void Awake()
    {
        vidaActual = vidaMax;
        RefrescarUI();
    }

    public void RecibirDaño(int cantidad)
    {
        // Si está invulnerable, ignorar el daño
        if (esInvulnerable || vidaActual <= 0)
            return;

        vidaActual = Mathf.Max(vidaActual - Mathf.Abs(cantidad), 0);
        RefrescarUI();

        if (vidaActual <= 0)
        {
            Morir();
        }
        else
        {
            // Activa invulnerabilidad temporal
            StartCoroutine(InvulnerabilidadTemporal());
        }
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

    System.Collections.IEnumerator InvulnerabilidadTemporal()
    {
        esInvulnerable = true;
        // Podés poner algún efecto visual acá (parpadeo, color, etc.)
        yield return new WaitForSeconds(tiempoInvulnerable);
        esInvulnerable = false;
    }

    void Morir()
    {
        Debug.Log("Jugador muerto");
        // TODO: respawn, animación o desactivar control
    }
}
