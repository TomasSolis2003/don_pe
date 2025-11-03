using UnityEngine;

public class VehicleTriggerRelay : MonoBehaviour
{
    public VehicleInteraction vehicle;

    void OnTriggerEnter(Collider other) => vehicle?.OnTriggerEnter(other);
    void OnTriggerExit(Collider other) => vehicle?.OnTriggerExit(other);
}
