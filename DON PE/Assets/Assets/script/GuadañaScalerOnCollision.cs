/*using UnityEngine;

public class UpgradeGuadañaScaler : MonoBehaviour
{
    public float scaleAmount = 1.1f; // Cantidad por la que se multiplicará la escala en X.

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto colisionado tiene el tag "Player".
        if (other.CompareTag("Player"))
        {
            // Busca el objeto con el tag "guadaña" en la escena.
            GameObject guadaña = GameObject.FindGameObjectWithTag("guadaña");

            if (guadaña != null)
            {
                // Aumenta la escala en X del objeto guadaña.
                Vector3 newScale = guadaña.transform.localScale;
                newScale.x *= scaleAmount;
                guadaña.transform.localScale = newScale;

                Debug.Log("La escala en X de la guadaña ha sido aumentada a: " + newScale.x);
            }
            else
            {
                Debug.LogWarning("No se encontró ningún objeto con el tag 'guadaña' en la escena.");
            }
        }
    }
}
*/
/*using UnityEngine;

public class UpgradeGuadañaScalerWithLimit : MonoBehaviour
{
    public float scaleAmount = 1.1f; // Cantidad por la que se multiplicará la escala en X.
    private int upgradeCount = 0; // Contador de mejoras.
    private int maxUpgrades = 10; // Límite de mejoras.
    private Renderer guadañaRenderer; // Referencia al Renderer de la guadaña para cambiar el color.

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject guadaña = GameObject.FindGameObjectWithTag("guadaña");

            if (guadaña != null)
            {
                // Aumenta la escala si aún no se ha alcanzado el límite.
                if (upgradeCount < maxUpgrades)
                {
                    Vector3 newScale = guadaña.transform.localScale;
                    newScale.x *= scaleAmount;
                    guadaña.transform.localScale = newScale;

                    upgradeCount++;
                    Debug.Log("Mejora " + upgradeCount + "/10 aplicada. Escala en X: " + newScale.x);

                    // Cambia gradualmente el color a medida que se acercan al límite.
                    guadañaRenderer = guadaña.GetComponent<Renderer>();
                    Color newColor = Color.Lerp(Color.white, Color.green, (float)upgradeCount / maxUpgrades);
                    guadañaRenderer.material.color = newColor;
                }
                else
                {
                    Debug.Log("Se alcanzó el máximo de mejoras.");
                }
            }
            else
            {
                Debug.LogWarning("No se encontró ningún objeto con el tag 'guadaña' en la escena.");
            }
        }
    }
}
*/
using UnityEngine;
using TMPro; // Necesario para usar Text Mesh Pro.

public class UpgradeGuadañaScalerWithText : MonoBehaviour
{
    public float scaleAmount = 1.1f; // Cantidad por la que se multiplicará la escala en X.
    private int upgradeCount = 0; // Contador de mejoras.
    private int maxUpgrades = 10; // Límite de mejoras.
    private Renderer upgradeRenderer; // Renderer del objeto UpgradeGuadaña.
    public TextMeshPro textMeshPro; // Referencia al Text Mesh Pro.

    private void Start()
    {
        upgradeRenderer = GetComponent<Renderer>();

        if (upgradeRenderer == null)
        {
            Debug.LogWarning("El objeto UpgradeGuadaña no tiene un Renderer asignado.");
        }

        // Inicializa el texto en "0/10".
        UpdateUpgradeText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject guadaña = GameObject.FindGameObjectWithTag("guadaña");

            if (guadaña != null && upgradeCount < maxUpgrades)
            {
                // Escalar la guadaña.
                Vector3 newScale = guadaña.transform.localScale;
                newScale.x *= scaleAmount;
                guadaña.transform.localScale = newScale;

                upgradeCount++;
                Debug.Log("Mejora " + upgradeCount + "/" + maxUpgrades + " aplicada.");

                // Actualizar el color de UpgradeGuadaña.
                Color newColor = Color.Lerp(Color.white, Color.green, (float)upgradeCount / maxUpgrades);
                if (upgradeRenderer != null)
                {
                    upgradeRenderer.material.color = newColor;
                }

                // Actualizar el texto.
                UpdateUpgradeText();
            }
            else
            {
                Debug.Log("Se alcanzó el máximo de mejoras.");
            }
        }
    }

    private void UpdateUpgradeText()
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = upgradeCount + "/" + maxUpgrades;
        }
        else
        {
            Debug.LogWarning("No se ha asignado un Text Mesh Pro.");
        }
    }
}
