using UnityEngine;

public class Jugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [Header("Cámara")]
    public float mouseSensitivity = 100f;
    public float mouseSmooth = 0.05f; // suavizado opcional
    public Transform playerBody;
    public Transform cameraTransform;

    private Rigidbody rb;

    private float xRotation = 0f;
    private bool isGrounded;

    // Suavizado
    private float smoothX, smoothY;
    private float smoothVelocityX, smoothVelocityY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = playerBody.GetComponent<Rigidbody>();
        rb.freezeRotation = true;  // evita que el rigidbody rote por colisiones
    }

    void Update()
    {
        if (!Cursor.visible)
        {
            LookAround();
            Move();
            Jump();
        }
    }

    // ------------------------------
    //      ROTACIÓN DE LA CÁMARA
    // ------------------------------
    void LookAround()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        // Suavizado (opcional)
        smoothX = Mathf.SmoothDamp(smoothX, mouseX, ref smoothVelocityX, mouseSmooth);
        smoothY = Mathf.SmoothDamp(smoothY, mouseY, ref smoothVelocityY, mouseSmooth);

        float rotX = smoothX * mouseSensitivity * Time.deltaTime;
        float rotY = smoothY * mouseSensitivity * Time.deltaTime;

        // Rotación vertical de la cámara
        xRotation -= rotY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotación horizontal del jugador
        playerBody.Rotate(Vector3.up * rotX);
    }

    // ------------------------------
    //      MOVIMIENTO
    // ------------------------------
    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 direction = (playerBody.forward * z + playerBody.right * x).normalized;

        Vector3 velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.z * moveSpeed);
        rb.velocity = velocity;
    }

    // ------------------------------
    //      SALTO
    // ------------------------------
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // Colisiones con el suelo
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}
