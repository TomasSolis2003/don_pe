using UnityEngine;

public class ArbolA : MonoBehaviour
{
    [Header("Vida del árbol")]
    public float maxHealth = 5f;
    private float currentHealth;

    [Header("Daño")]
    public float hitDamage = 1f;
    public float hitCooldown = 0.2f;
    private bool canTakeDamage = true;

    [Header("Shake (temblor)")]
    public float shakeAmount = 0.1f;
    public float shakeSpeed = 15f;
    private bool isShaking = false;
    private Vector3 initialRotation;

    [Header("Destrucción")]
    public float fallSpeed = 50f;
    private bool isFalling = false;

    private void Start()
    {
        currentHealth = maxHealth;
        initialRotation = transform.eulerAngles;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Cualquier objeto con Tag "Player" o "Axe"
        if (other.CompareTag("Hacha")&&(other.GetComponent<HachaA>().isAttacking))
        {
            TakeDamage(hitDamage);
        }
    }

    void TakeDamage(float amount)
    {
        if (!canTakeDamage) return;

        currentHealth -= amount;
        StartCoroutine(DamageCooldown());
        StartCoroutine(ShakeTree());

        if (currentHealth <= 0 && !isFalling)
        {
            StartCoroutine(FallAndDestroy());
        }
    }

    System.Collections.IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(hitCooldown);
        canTakeDamage = true;
    }

    System.Collections.IEnumerator ShakeTree()
    {
        isShaking = true;

        float t = 0;
        while (t < 0.2f)
        {
            float shake = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            transform.eulerAngles = initialRotation + new Vector3(0, 0, shake * 10f);
            t += Time.deltaTime;
            yield return null;
        }

        transform.eulerAngles = initialRotation;
        isShaking = false;
    }

    System.Collections.IEnumerator FallAndDestroy()
    {
        isFalling = true;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(
            transform.eulerAngles.x + 90f,   // cae hacia adelante
            transform.eulerAngles.y,
            transform.eulerAngles.z
        );

        float t = 0;

        // Animación de caída
        while (t < 1f)
        {
            t += Time.deltaTime * 0.7f; // velocidad de caída
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        // 🔥 Esperar X segundos en el piso antes de desaparecer
        yield return new WaitForSeconds(2f);  // <-- CAMBIA ESTE VALOR A LO QUE QUIERAS

        Destroy(gameObject);
    }

}
