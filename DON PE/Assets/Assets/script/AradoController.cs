using UnityEngine;

public class AradoController : MonoBehaviour
{
    public string unpreparedSoilTag = "UnpreparedSoil"; // Tag para identificar la tierra sin preparar
    public GameObject preparedSoilPrefab; // Prefab de la tierra preparada
    public float speed = 10f;
    public float turnSpeed = 30f;

    private void Update()
    {
        // Movimiento del arador
        float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        transform.Translate(Vector3.forward * move);
        transform.Rotate(Vector3.up * turn);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(unpreparedSoilTag))
        {
            Vector3 position = other.transform.position;
            Quaternion rotation = other.transform.rotation;
            Destroy(other.gameObject); // Elimina la tierra sin preparar
            Instantiate(preparedSoilPrefab, position, rotation); // Genera la tierra preparada
        }
    }
}
