using UnityEngine;
using System.Collections;

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
    private Vector3 initialRotation;

    [Header("Destrucción / Respawn")]
    public float timeOnGround = 1f;
    public float respawnTime = 5f;

    [Header("Loot")]
    public GameObject troncoPrefab;

    private bool isFalling = false;

    // Referencias
    private MeshRenderer mesh;
    private Collider col;
    private Quaternion initialRotQ;
    private Vector3 initialPos;

    private void Start()
    {
        currentHealth = maxHealth;
        initialRotation = transform.eulerAngles;
        initialRotQ = transform.rotation;
        initialPos = transform.position;

        mesh = GetComponentInChildren<MeshRenderer>();
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hacha") && other.GetComponent<HachaA>().isAttacking)
        {
            TakeDamage(hitDamage);
        }
    }

    void TakeDamage(float amount)
    {
        if (!canTakeDamage || isFalling) return;

        currentHealth -= amount;

        StartCoroutine(DamageCooldown());
        StartCoroutine(ShakeTree());

        if (currentHealth <= 0)
        {
            StartCoroutine(FallAndRespawn());
        }
    }

    IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(hitCooldown);
        canTakeDamage = true;
    }

    IEnumerator ShakeTree()
    {
        float t = 0;
        while (t < 0.2f)
        {
            float shake = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            transform.eulerAngles = initialRotation + new Vector3(0, 0, shake * 10f);
            t += Time.deltaTime;
            yield return null;
        }

        transform.eulerAngles = initialRotation;
    }

    IEnumerator FallAndRespawn()
    {
        isFalling = true;
        col.enabled = false;

        // ANIMACIÓN DE CAÍDA
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(
            transform.eulerAngles.x + 90f,
            transform.eulerAngles.y,
            transform.eulerAngles.z
        );

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 0.7f;
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        // Queda tirado en el piso
        yield return new WaitForSeconds(timeOnGround);

        // ❗ Spawn de troncos
        SpawnTroncosCaidos();

        // Ocultar árbol
        mesh.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        RespawnTree();
    }

   void SpawnTroncosCaidos()
{
    if (troncoPrefab == null) return;

    // Rotación acostada (90° en X)
    Quaternion rot = Quaternion.Euler(90f, transform.eulerAngles.y, 0f);

    float startOffset = 0.3f;   // hacia adelante desde el centro
    float spacing = 0.8f;       // distancia entre troncos
    float heightOffset = 0.2f;  // ALTURA para que caigan con RB

    // Punto inicial: centro + adelante + arriba
    Vector3 startPos = transform.position +
                       transform.forward * startOffset +
                       Vector3.up * heightOffset;

    for (int i = 0; i < 3; i++)
    {
        Vector3 pos = startPos + transform.forward * (i * spacing);
        Instantiate(troncoPrefab, pos, rot);
    }
}



    void RespawnTree()
    {
        currentHealth = maxHealth;
        isFalling = false;

        transform.position = initialPos;
        transform.rotation = initialRotQ;

        mesh.enabled = true;
        col.enabled = true;
    }
}