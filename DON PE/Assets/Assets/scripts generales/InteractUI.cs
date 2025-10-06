/*using UnityEngine;
using TMPro;

public class InteractUI : MonoBehaviour
{
    [Header("Player Settings")]
    [Tooltip("Tag del jugador.")]
    public string playerTag = "Player";

    [Header("UI Elements")]
    [Tooltip("Texto para mostrar el mensaje de interacción.")]
    public TextMeshProUGUI interactionText;

    [Tooltip("Texto para mostrar el punto de interacción.")]
    public TextMeshProUGUI dotText;

    [Header("Distancia de Interacción")]
    [Tooltip("Distancia máxima para mostrar el mensaje de interacción.")]
    public float interactionDistance = 3f;

    private Transform playerTransform;

    private void Start()
    {
        if (interactionText != null)
        {
            interactionText.text = ""; // Ocultar al inicio
        }
        if (dotText != null)
        {
            dotText.text = "."; // Mostrar el punto al inicio
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);

            if (distance <= interactionDistance)
            {
                if (interactionText != null)
                {
                    interactionText.text = "Press F for interact";
                }
                if (dotText != null)
                {
                    dotText.text = ""; // Ocultar el punto
                }
            }
            else
            {
                if (interactionText != null)
                {
                    interactionText.text = ""; // Ocultar el mensaje de interacción
                }
                if (dotText != null)
                {
                    dotText.text = "."; // Mostrar el punto
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerTransform = null;
            if (interactionText != null)
            {
                interactionText.text = ""; // Ocultar al salir
            }
            if (dotText != null)
            {
                dotText.text = "."; // Mostrar el punto
            }
        }
    }
}*/
using UnityEngine;
using TMPro;

public class InteractUI : MonoBehaviour
{
    [Header("Player Settings")]
    [Tooltip("Tag del jugador.")]
    public string playerTag = "Player";

    [Header("UI Elements")]
    [Tooltip("Texto para mostrar el mensaje de interacción.")]
    public TextMeshProUGUI interactionText;

    [Tooltip("Texto para mostrar el punto de interacción.")]
    public TextMeshProUGUI dotText;

    [Header("Distancia de Interacción")]
    [Tooltip("Distancia máxima para mostrar el mensaje de interacción.")]
    public float interactionDistance = 3f;

    private Transform playerTransform;

    private void Start()
    {
        if (interactionText != null)
        {
            interactionText.text = ""; // Ocultar al inicio
        }
        if (dotText != null)
        {
            dotText.text = "."; // Mostrar el punto al inicio
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);

            if (distance <= interactionDistance)
            {
                if (interactionText != null)
                {
                    interactionText.text = "Press F for interact";
                }
                if (dotText != null)
                {
                    dotText.text = ""; // Ocultar el punto
                }
            }
            else
            {
                if (interactionText != null)
                {
                    interactionText.text = ""; // Ocultar el mensaje de interacción
                }
                if (dotText != null)
                {
                    dotText.text = "."; // Mostrar el punto
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerTransform = null;
            if (interactionText != null)
            {
                interactionText.text = ""; // Ocultar al salir
            }
            if (dotText != null)
            {
                dotText.text = "."; // Mostrar el punto
            }
        }
    }
}

