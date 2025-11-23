using UnityEngine;

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
