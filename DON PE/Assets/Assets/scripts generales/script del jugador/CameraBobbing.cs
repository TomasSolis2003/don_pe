using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    public float walkBobSpeed = 8f;      // Velocidad de la oscilación al caminar
    public float walkBobAmount = 0.05f;  // Intensidad del bobbing al caminar

    public float idleBobSpeed = 1.5f;    // Velocidad del bobbing en reposo (respiración)
    public float idleBobAmount = 0.02f;  // Intensidad del bobbing en reposo

    private float defaultY;              // Altura inicial de la cámara
    private float timer = 0f;            // Reloj interno del bobbing

    void Start()
    {
        defaultY = transform.localPosition.y;
    }

    void Update()
    {
        bool moving = Input.GetKey(KeyCode.W) ||
                      Input.GetKey(KeyCode.A) ||
                      Input.GetKey(KeyCode.S) ||
                      Input.GetKey(KeyCode.D);

        if (moving)
        {
            timer += Time.deltaTime * walkBobSpeed;
            float newY = defaultY + Mathf.Sin(timer) * walkBobAmount;
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
        else
        {
            timer += Time.deltaTime * idleBobSpeed;
            float newY = defaultY + Mathf.Sin(timer) * idleBobAmount;
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
    }
}

/*using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    [Header("Movimiento al caminar")]
    public float walkSpeed = 6f;       // Velocidad del ciclo al caminar
    public float bobAmount = 0.03f;    // Bobbing vertical
    public float swayAmount = 1f;      // Inclinación lateral (roll)

    [Header("Idle (quieto)")]
    public float idleMultiplier = 0.25f;  // Qué tan suave es el idle

    private float timer = 0f;
    private Vector3 defaultPos;

    void Start()
    {
        defaultPos = transform.localPosition;
    }

    void Update()
    {
        bool moving =
            Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D);

        float speed = moving ? walkSpeed : walkSpeed * idleMultiplier;
        float sway = moving ? swayAmount : swayAmount * idleMultiplier;
        float bob = moving ? bobAmount : bobAmount * idleMultiplier;

        timer += Time.deltaTime * speed;

        // Bobbing vertical suave (posición)
        float yOffset = Mathf.Sin(timer) * bob;
        transform.localPosition = defaultPos + new Vector3(0, yOffset, 0);

        // Sway lateral (rotación en Z) sin tocar pitch/yaw
        float zRot = Mathf.Sin(timer) * sway;
        transform.localRotation = Quaternion.Euler(0, 0, zRot);
    }
}
*/