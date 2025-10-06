using UnityEngine;

public class DiabloBehavior : MonoBehaviour
{
    private float interval;
    private float timer;

    public void Initialize(float boxColliderInterval)
    {
        interval = boxColliderInterval;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            SpawnDestroyBox();
            timer = 0f;
        }
    }

    private void SpawnDestroyBox()
    {
        GameObject box = new GameObject("DestroyBox");
        BoxCollider boxCollider = box.AddComponent<BoxCollider>();
        box.transform.position = transform.position;
        box.transform.localScale = new Vector3(5, 5, 5); // Ajusta el tamaño según sea necesario
        boxCollider.isTrigger = true;

        DestroyBox destroyBox = box.AddComponent<DestroyBox>();
        destroyBox.lifeTime = 2f; // Duración del BoxCollider antes de destruirse
    }
}

public class DestroyBox : MonoBehaviour
{
    public float lifeTime = 2f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("trigo"))
        {
            Destroy(other.gameObject);
        }
    }
}
