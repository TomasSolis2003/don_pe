using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Transform player;
    public float speed = 2f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        DayNightCycle cycle = FindObjectOfType<DayNightCycle>();

        if (cycle != null && cycle.enabled)
        {
            if (cycle.enabled) // Solo se mueve si es de noche
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
        }
    }
}
