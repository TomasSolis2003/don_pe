using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InvYcons : MonoBehaviour
{
    [Header("Inventario")]
    public int troncos = 0; // Cantidad de troncos en inventario
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

    void ActualizarUI()
    {
        if (troncosTexto != null)
            troncosTexto.text = "Troncos: " + troncos;
    }

}
