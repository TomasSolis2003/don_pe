
using UnityEngine;

public class TransformFixer : MonoBehaviour
{
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Verificar y corregir la escala
            if (player.transform.localScale != Vector3.one)
            {
                Debug.LogWarning("¡Advertencia! La escala del jugador no es (1,1,1). Corrigiendo...");
                player.transform.localScale = Vector3.one;
            }

            // Obtener la rotación actual
            Vector3 currentRotation = player.transform.eulerAngles;

            // Corregir solo los ejes X y Z, dejando Y libre
            if (currentRotation.x != 0f || currentRotation.z != 0f)
            {
                Debug.LogWarning("¡Advertencia! La rotación en X/Z no es 0. Corrigiendo...");
                player.transform.rotation = Quaternion.Euler(0f, currentRotation.y, 0f);
            }
        }
        else
        {
            Debug.LogError("No se encontró un GameObject con el tag 'Player'.");
        }
    }
}
