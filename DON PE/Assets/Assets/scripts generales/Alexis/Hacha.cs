using UnityEngine;

public class HachaA : MonoBehaviour
{
    public Animator animator;
    public string attackBool = "isAttacking";

    public float attackDuration = 0.4f;
    public bool isAttacking = false;
    public int hitDamage = 20;
    public Collider hitbox;

    private Quaternion originalRotation;
    private Vector3 originalPosition;

    void Start()
    {
        originalRotation = transform.localRotation;
        originalPosition = transform.localPosition;
        hitbox.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking && !Cursor.visible)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private System.Collections.IEnumerator AttackRoutine()
    {
        hitbox.enabled = true;
        isAttacking = true;

        if (animator != null)
            animator.SetBool(attackBool, true);

        // Rotación del golpe
        Quaternion attackRotation = Quaternion.Euler(0f, 50f, 40f);

        // Movimiento hacia la izquierda (X negativo)
        Vector3 attackPosition = originalPosition + new Vector3(-0.2f, 0f, 0.2f);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 10;

            transform.localRotation = Quaternion.Slerp(originalRotation, attackRotation, t);
            transform.localPosition = Vector3.Lerp(originalPosition, attackPosition, t);

            yield return null;
        }

        yield return new WaitForSeconds(attackDuration);

        // Regresa a la rotación y posición original
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 10;

            transform.localRotation = Quaternion.Slerp(attackRotation, originalRotation, t);
            transform.localPosition = Vector3.Lerp(attackPosition, originalPosition, t);

            yield return null;
        }

        if (animator != null)
            animator.SetBool(attackBool, false);

        isAttacking = false;
        hitbox.enabled = false;
    }
}
