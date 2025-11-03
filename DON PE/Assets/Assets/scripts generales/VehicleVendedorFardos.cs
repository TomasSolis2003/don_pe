using UnityEngine;

public class VehicleVendedorFardos : MonoBehaviour
{
    [Header("Configuración de venta")]
    [SerializeField] private int precioPorFardo = 25;
    [SerializeField] private string tagFardo = "fardo";
    [SerializeField] private AudioSource sonidoVenta; // Opcional
    [SerializeField] private ParticleSystem efectoVenta; // Opcional

    [Header("Sistema de dinero")]
    [SerializeField] private UpgradeGuadañaScalerWithTextAndColor sistemaDinero; // referencia al script que tiene el dinero

    private void Start()
    {
        if (sistemaDinero == null)
            Debug.LogWarning("⚠️ No se asignó el sistema de dinero. Asigná el script con la variable 'dinero'.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tagFardo)) return;

        // Efecto visual y sonido
        if (efectoVenta != null) efectoVenta.Play();
        if (sonidoVenta != null) sonidoVenta.Play();

        // Vender fardo
        VenderFardo(other.gameObject);
    }

    private void VenderFardo(GameObject fardo)
    {
        if (sistemaDinero == null)
        {
            Debug.LogWarning("💰 No hay sistema de dinero asignado.");
            return;
        }

        sistemaDinero.AgregarDinero(precioPorFardo);
        Destroy(fardo);
        Debug.Log($"🟢 Fardo vendido por ${precioPorFardo}");
    }
}
