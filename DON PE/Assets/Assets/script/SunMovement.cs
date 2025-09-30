using UnityEngine;

public class SunMovement : MonoBehaviour
{
    public float dayDuration = 60f; // Duraci�n del d�a en segundos (aj�stalo seg�n necesites)

    void Update()
    {
        float rotationSpeed = 360f / dayDuration; // Grados por segundo
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}
