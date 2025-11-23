/*using UnityEngine;

public class DamageReceiver : MonoBehaviour, IDañoRecibible
{
    private AnimalIA animal;

    void Start()
    {
        animal = GetComponentInParent<AnimalIA>();
    }

    public void RecibirDaño(int cantidad) // ← AHORA COINCIDE CON LA INTERFAZ
    {
        if (animal != null)
            animal.RecibirDaño(cantidad);
    }
}
*/
using UnityEngine;

public class DamageReceiver : MonoBehaviour, IDañoRecibible
{
    private AnimalIA animal;

    void Awake()
    {
        animal = GetComponentInParent<AnimalIA>();
    }

    public void RecibirDaño(int cantidad)
    {
        Debug.Log($"[DamageReceiver] Recibí daño {cantidad} en {name}");
        if (animal != null)
            animal.RecibirDaño(cantidad);
        else
            Debug.LogWarning("[DamageReceiver] No encontré AnimalIA en padres");
    }
}
