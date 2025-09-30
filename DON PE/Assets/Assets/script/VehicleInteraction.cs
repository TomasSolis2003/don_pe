
/*using UnityEngine;

public class VehicleInteraction : MonoBehaviour
{
    public Transform playerSeat; // Asiento del vehículo
    public GameObject vehicleCamera; // Cámara del vehículo
    public GameObject playerCamera; // Cámara del jugador
    public MonoBehaviour playerControllerScript; // Script del control del jugador
    public MonoBehaviour vehicleControlScript; // Script del control del vehículo
    public GameObject interactionArea; // Área de interacción para detectar al jugador

    private Transform player; // Referencia al jugador
    private bool isPlayerInVehicle = false;

    void Start()
    {
     

        if (vehicleCamera != null)
            vehicleCamera.SetActive(false); // Desactiva la cámara del vehículo al inicio

        if (vehicleControlScript != null)
            vehicleControlScript.enabled = false; // Desactiva el control del vehículo al inicio

        if (interactionArea != null)
        {
            Collider collider = interactionArea.GetComponent<Collider>();
            if (collider != null)
                collider.isTrigger = true; // Asegura que el área de interacción sea un Trigger
        }
    }

    void Update()
    {
        if (player == null) return;

        // Acción de subir o bajar con la tecla F
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isPlayerInVehicle)
            {
                HandleExitVehicle();
            }
            else
            {
                HandleEnterVehicle();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform; // Detecta al jugador
            Debug.Log("Jugador detectado en el área de interacción.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isPlayerInVehicle) // Solo borra referencia si no está en el vehículo
            {
                player = null;
                Debug.Log("Jugador salió del área de interacción.");
            }
        }
    }

    private void HandleEnterVehicle()
    {
        if (player == null) return;

        Debug.Log("Subiendo al vehículo...");

        // Desactiva el control del jugador y su cámara
        if (playerControllerScript != null)
            playerControllerScript.enabled = false;

        if (playerCamera != null)
            playerCamera.SetActive(false);

        // Ancla al jugador al asiento
        player.SetParent(playerSeat);
        player.position = playerSeat.position;
        player.rotation = playerSeat.rotation;

        // Activa el control del vehículo y su cámara
        if (vehicleControlScript != null)
            vehicleControlScript.enabled = true;

        if (vehicleCamera != null)
            vehicleCamera.SetActive(true);

        isPlayerInVehicle = true;
    }

    private void HandleExitVehicle()
    {
        if (player == null) return;

        Debug.Log("Bajando del vehículo...");

        // Reactiva el control del jugador y su cámara
        if (playerControllerScript != null)
            playerControllerScript.enabled = true;

        if (playerCamera != null)
            playerCamera.SetActive(true);

        // Libera al jugador del asiento
        player.SetParent(null);
        player.position = transform.position + Vector3.right * 2; // Lo coloca al lado del vehículo

        // Desactiva el control del vehículo y su cámara
        if (vehicleControlScript != null)
            vehicleControlScript.enabled = false;

        if (vehicleCamera != null)
            vehicleCamera.SetActive(false);

        isPlayerInVehicle = false;
    }
}
*/
using UnityEngine;

public class VehicleInteraction : MonoBehaviour
{
    public Transform playerSeat; // Asiento del vehículo
    public GameObject vehicleCamera; // Cámara del vehículo
    public GameObject playerCamera; // Cámara del jugador
    public MonoBehaviour playerControllerScript; // Script del control del jugador
    public MonoBehaviour vehicleControlScript; // Script del control del vehículo
    public GameObject interactionArea; // Área de interacción para detectar al jugador

    private Transform player; // Referencia al jugador
    private bool isPlayerInVehicle = false;

    void Start()
    {
        if (vehicleCamera != null)
            vehicleCamera.SetActive(false); // Desactiva la cámara del vehículo al inicio

        if (vehicleControlScript != null)
            vehicleControlScript.enabled = false; // Desactiva el control del vehículo al inicio

        if (interactionArea != null)
        {
            Collider collider = interactionArea.GetComponent<Collider>();
            if (collider != null)
                collider.isTrigger = true; // Asegura que el área de interacción sea un Trigger
        }
    }

    void Update()
    {
        if (player == null) return;

        // Acción de subir o bajar con la tecla F
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isPlayerInVehicle)
            {
                HandleExitVehicle();
            }
            else
            {
                HandleEnterVehicle();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform; // Detecta al jugador
            Debug.Log("Jugador detectado en el área de interacción.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isPlayerInVehicle) // Solo borra referencia si no está en el vehículo
            {
                player = null;
                Debug.Log("Jugador salió del área de interacción.");
            }
        }
    }

    private void HandleEnterVehicle()
    {
        if (player == null) return;

        Debug.Log("Subiendo al vehículo...");

        // Desactiva el control del jugador y su cámara
        if (playerControllerScript != null)
            playerControllerScript.enabled = false;

        if (playerCamera != null)
            playerCamera.SetActive(false);

        // Ancla al jugador al asiento
        player.SetParent(playerSeat);
        player.position = playerSeat.position;
        player.rotation = playerSeat.rotation;

        // Activa el control del vehículo y su cámara
        if (vehicleControlScript != null)
            vehicleControlScript.enabled = true;

        if (vehicleCamera != null)
            vehicleCamera.SetActive(true);

        isPlayerInVehicle = true;
    }

    private void HandleExitVehicle()
    {
        if (player == null) return;

        Debug.Log("Bajando del vehículo...");

        // Reactiva el control del jugador y su cámara
        if (playerControllerScript != null)
            playerControllerScript.enabled = true;

        if (playerCamera != null)
            playerCamera.SetActive(true);

        // Libera al jugador del asiento
        player.SetParent(null);
        player.position = transform.position + Vector3.right * 2; // Lo coloca al lado del vehículo

        // Desactiva el control del vehículo y su cámara
        if (vehicleControlScript != null)
            vehicleControlScript.enabled = false;

        if (vehicleCamera != null)
            vehicleCamera.SetActive(false);

        isPlayerInVehicle = false;
    }
}
