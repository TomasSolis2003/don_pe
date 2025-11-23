using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InvYcons : MonoBehaviour
{
    [Header("Inventario")]
    public int troncos = 0;
    public int bayas = 0;
    public TextMeshProUGUI troncosTexto;

    [Header("UI")]
    public GameObject panelConstruccion;
    public bool construccionActiva = false;
    private void Start()
    {
        panelConstruccion.SetActive(false);
    }

    void Update()
    {
        ActualizarUI();

        if (Input.GetKeyDown(KeyCode.P))
        {
            ModoConstruccion();
        }
    }
    void ActualizarUI()
    {
        if (troncosTexto != null)
            troncosTexto.text = "Troncos: " + troncos;
    }
    public void Almacenar(string item, int cantidad)
    {
        if (item == "tronco")
        {
            troncos += cantidad;
        }
        else if (item == "baya")
        {
            bayas += cantidad;
        }
    }
    public void ModoConstruccion()
    {
        construccionActiva = !construccionActiva;

        panelConstruccion.SetActive(construccionActiva);
        Cursor.visible = construccionActiva;
        if (construccionActiva)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
