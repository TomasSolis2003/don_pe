
/*using UnityEngine;

public class VehicleInteraction : MonoBehaviour
{
    public Transform playerSeat; // Asiento del veh�culo
    public GameObject vehicleCamera; // C�mara del veh�culo
    public GameObject playerCamera; // C�mara del jugador
    public MonoBehaviour playerControllerScript; // Script del control del jugador
    public MonoBehaviour vehicleControlScript; // Script del control del veh�culo
    public GameObject interactionArea; // �rea de interacci�n para detectar al jugador

    private Transform player; // Referencia al jugador
    private bool isPlayerInVehicle = false;

    void Start()
    {
     

        if (vehicleCamera != null)
            vehicleCamera.SetActive(false); // Desactiva la c�mara del veh�culo al inicio

        if (vehicleControlScript != null)
            vehicleControlScript.enabled = false; // Desactiva el control del veh�culo al inicio

        if (interactionArea != null)
        {
            Collider collider = interactionArea.GetComponent<Collider>();
            if (collider != null)
                collider.isTrigger = true; // Asegura que el �rea de interacci�n sea un Trigger
        }
    }

    void Update()
    {
        if (player == null) return;

        // Acci�n de subir o bajar con la tecla F
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
            Debug.Log("Jugador detectado en el �rea de interacci�n.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isPlayerInVehicle) // Solo borra referencia si no est� en el veh�culo
            {
                player = null;
                Debug.Log("Jugador sali� del �rea de interacci�n.");
            }
        }
    }

    private void HandleEnterVehicle()
    {
        if (player == null) return;

        Debug.Log("Subiendo al veh�culo...");

        // Desactiva el control del jugador y su c�mara
        if (playerControllerScript != null)
            playerControllerScript.enabled = false;

        if (playerCamera != null)
            playerCamera.SetActive(false);

        // Ancla al jugador al asiento
        player.SetParent(playerSeat);
        player.position = playerSeat.position;
        player.rotation = playerSeat.rotation;

        // Activa el control del veh�culo y su c�mara
        if (vehicleControlScript != null)
            vehicleControlScript.enabled = true;

        if (vehicleCamera != null)
            vehicleCamera.SetActive(true);

        isPlayerInVehicle = true;
    }

    private void HandleExitVehicle()
    {
        if (player == null) return;

        Debug.Log("Bajando del veh�culo...");

        // Reactiva el control del jugador y su c�mara
        if (playerControllerScript != null)
            playerControllerScript.enabled = true;

        if (playerCamera != null)
            playerCamera.SetActive(true);

        // Libera al jugador del asiento
        player.SetParent(null);
        player.position = transform.position + Vector3.right * 2; // Lo coloca al lado del veh�culo

        // Desactiva el control del veh�culo y su c�mara
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
    public Transform playerSeat; // Asiento del veh�culo
    public GameObject vehicleCamera; // C�mara del veh�culo
    public GameObject playerCamera; // C�mara del jugador
    public MonoBehaviour playerControllerScript; // Script del control del jugador
    public MonoBehaviour vehicleControlScript; // Script del control del veh�culo
    public GameObject interactionArea; // �rea de interacci�n para detectar al jugador

    private Transform player; // Referencia al jugador
    private bool isPlayerInVehicle = false;

    void Start()
    {
        if (vehicleCamera != null)
            vehicleCamera.SetActive(false); // Desactiva la c�mara del veh�culo al inicio

        if (vehicleControlScript != null)
            vehicleControlScript.enabled = false; // Desactiva el control del veh�culo al inicio

        if (interactionArea != null)
        {
            Collider collider = interactionArea.GetComponent<Collider>();
            if (collider != null)
                collider.isTrigger = true; // Asegura que el �rea de interacci�n sea un Trigger
        }
    }

    void Update()
    {
        if (player == null) return;

        // Acci�n de subir o bajar con la tecla F
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
            Debug.Log("Jugador detectado en el �rea de interacci�n.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isPlayerInVehicle) // Solo borra referencia si no est� en el veh�culo
            {
                player = null;
                Debug.Log("Jugador sali� del �rea de interacci�n.");
            }
        }
    }

    private void HandleEnterVehicle()
    {
        if (player == null) return;

        Debug.Log("Subiendo al veh�culo...");

        // Desactiva el control del jugador y su c�mara
        if (playerControllerScript != null)
            playerControllerScript.enabled = false;

        if (playerCamera != null)
            playerCamera.SetActive(false);

        // Ancla al jugador al asiento
        player.SetParent(playerSeat);
        player.position = playerSeat.position;
        player.rotation = playerSeat.rotation;

        // Activa el control del veh�culo y su c�mara
        if (vehicleControlScript != null)
            vehicleControlScript.enabled = true;

        if (vehicleCamera != null)
            vehicleCamera.SetActive(true);

        isPlayerInVehicle = true;
    }

    private void HandleExitVehicle()
    {
        if (player == null) return;

        Debug.Log("Bajando del veh�culo...");

        // Reactiva el control del jugador y su c�mara
        if (playerControllerScript != null)
            playerControllerScript.enabled = true;

        if (playerCamera != null)
            playerCamera.SetActive(true);

        // Libera al jugador del asiento
        player.SetParent(null);
        player.position = transform.position + Vector3.right * 2; // Lo coloca al lado del veh�culo

        // Desactiva el control del veh�culo y su c�mara
        if (vehicleControlScript != null)
            vehicleControlScript.enabled = false;

        if (vehicleCamera != null)
            vehicleCamera.SetActive(false);

        isPlayerInVehicle = false;
    }
}
