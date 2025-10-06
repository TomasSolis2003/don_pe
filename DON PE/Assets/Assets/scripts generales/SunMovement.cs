using UnityEngine;

public class SunMovement : MonoBehaviour
{
    public float dayDuration = 60f; // Duración del día en segundos (ajústalo según necesites)

    void Update()
    {
        float rotationSpeed = 360f / dayDuration; // Grados por segundo
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}
