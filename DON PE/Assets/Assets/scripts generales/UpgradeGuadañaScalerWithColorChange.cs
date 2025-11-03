
using UnityEngine;
using TMPro;

public class UpgradeGuadañaScalerWithTextAndColor : MonoBehaviour
{
    [Header("Mejoras de la guadaña")]
    [SerializeField] private GameObject guadaña;
    [SerializeField] private float scaleAmount = 1.1f;
    [SerializeField] private int maxUpgrades = 10;
    [SerializeField] private TextMeshProUGUI upgradeText;

    private int upgradeCount = 0;
    private Renderer upgradeRenderer;

    [Header("Sistema de dinero")]
    [SerializeField] private int dinero = 100;
    [SerializeField] private int costoMejora = 50;
    [SerializeField] private TextMeshProUGUI dineroText;

    [Header("Visual")]
    [SerializeField] private bool usarMaterialCompartido = true;
    [SerializeField] private bool activarEfectoFlash = true;

    private void Start()
    {
        // Cachear el renderer
        upgradeRenderer = GetComponent<Renderer>();
        if (upgradeRenderer == null)
            Debug.LogWarning("⚠️ No se encontró Renderer en " + name);

        // Cachear guadaña si no está asignada
        if (guadaña == null)
            guadaña = GameObject.FindGameObjectWithTag("guadaña");

        UpdateUpgradeText();
        UpdateDineroUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                IntentarMejorarGuadana();
                break;

            case "mercado":
                ComprarEnMercado();
                break;
        }
    }

    // ============================================================
    // 🔧 LÓGICA DE MEJORA
    // ============================================================
    private void IntentarMejorarGuadana()
    {
        if (guadaña == null)
        {
            Debug.LogWarning("No se asignó la guadaña.");
            return;
        }

        if (upgradeCount >= maxUpgrades)
        {
            Debug.Log("🟢 La guadaña ya está al máximo nivel.");
            return;
        }

        if (!PuedePagar(costoMejora))
            return;

        // Escalar solo en X
        Vector3 newScale = guadaña.transform.localScale;
        newScale.x *= scaleAmount;
        guadaña.transform.localScale = newScale;

        upgradeCount++;
        Debug.Log($"Mejora aplicada: {upgradeCount}/{maxUpgrades}. Dinero restante: ${dinero}");

        // Cambiar color progresivamente
        float progreso = Mathf.Clamp01((float)upgradeCount / maxUpgrades);
        Color newColor = Color.Lerp(Color.white, Color.green, progreso);
        AplicarColor(newColor);

        // Pequeño efecto visual
        if (activarEfectoFlash)
            StartCoroutine(FlashUpgradeColor());

        UpdateUpgradeText();
        UpdateDineroUI();
    }

    private bool PuedePagar(int costo)
    {
        if (dinero >= costo)
        {
            dinero -= costo;
            return true;
        }

        Debug.Log("❌ Dinero insuficiente para mejorar.");
        return false;
    }

    // ============================================================
    // 🛒 MERCADO
    // ============================================================
    private void ComprarEnMercado()
    {
        const int costo = 50;

        if (!PuedePagar(costo))
        {
            Debug.Log("❌ No tienes suficiente dinero para comprar en el mercado.");
            return;
        }

        Debug.Log($"🪙 Compra realizada por ${costo}. Dinero restante: ${dinero}");
        UpdateDineroUI();
    }

    // ============================================================
    // 🎨 VISUAL / UI
    // ============================================================
    private void AplicarColor(Color color)
    {
        if (upgradeRenderer == null) return;

        if (usarMaterialCompartido)
            upgradeRenderer.sharedMaterial.color = color;
        else
            upgradeRenderer.material.color = color;
    }

    private void UpdateUpgradeText()
    {
        if (upgradeText != null)
            upgradeText.text = $"{upgradeCount}/{maxUpgrades}";
    }

    private void UpdateDineroUI()
    {
        if (dineroText != null)
            dineroText.text = $"Dinero: ${dinero}";
    }

    private System.Collections.IEnumerator FlashUpgradeColor()
    {
        if (upgradeRenderer == null) yield break;

        Color original = upgradeRenderer.sharedMaterial.color;
        upgradeRenderer.sharedMaterial.color = Color.yellow;
        yield return new WaitForSeconds(0.15f);
        upgradeRenderer.sharedMaterial.color = original;
    }
    public void AgregarDinero(int cantidad)
    {
        dinero += cantidad;
        UpdateDineroUI();
        Debug.Log($"💵 Dinero agregado: +${cantidad}. Total: ${dinero}");
    }
}
