using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InvYcons : MonoBehaviour
{
    [Header("Inventario")]
    public int troncos = 0; // Cantidad de troncos en inventario
    public TextMeshProUGUI troncosTexto;

    [Header("Construcción")]
    public GameObject prefabConstruccion; // Prefab que se va a construir
    public LayerMask sueloLayer; // Layer del suelo
    public float alturaMinima = 0f; // No construir por debajo de esta altura

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

        if (Input.GetMouseButtonDown(0)) // Click izquierdo
        {
            IntentarConstruir();
        }
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

    void IntentarConstruir()
    {
        if (troncos <= 0)
        {
            Debug.Log("No tienes suficientes troncos.");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Detecta si el raycast golpea el suelo
        if (Physics.Raycast(ray, out hit, 100f, sueloLayer))
        {
            Vector3 posicionConstruccion = hit.point;

            // Evita construir por debajo del suelo
            if (posicionConstruccion.y < alturaMinima)
            {
                Debug.Log("No se puede construir más abajo del suelo.");
                return;
            }

            // Instancia el objeto
            Instantiate(prefabConstruccion, posicionConstruccion, Quaternion.identity);

            // Resta los troncos necesarios (1 por ejemplo)
            troncos--;
        }
        else
        {
            Debug.Log("Solo puedes construir sobre el suelo.");
        }
    }
}
