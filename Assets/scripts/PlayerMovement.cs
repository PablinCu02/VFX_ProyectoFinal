using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController characterController;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float speed = 12f;
    public float gravity = -19.81f; // RECUERDA: Debe ser negativo
    public float jumpHeight = 1f;
    public float sphereRadius = 0.4f;

    Vector3 velocity;
    bool isGrounded;

    void Update()
    {
        // 1. Protección de referencia
        if (groundCheck == null || characterController == null)
        {
            Debug.LogError("¡Falta asignar el GroundCheck o el CharacterController en el Inspector!");
            return;
        }

        // 2. Detección de suelo
        isGrounded = Physics.CheckSphere(groundCheck.position, sphereRadius, groundMask);

        // --- DEBUG PARA DETECTAR EL FALLO ---
        if (Input.GetButtonDown("Jump"))
        {
            if (!isGrounded) Debug.LogWarning("Intentaste saltar pero el script cree que NO estás tocando el suelo.");
            else Debug.Log("Saltando con éxito.");
        }
        // ------------------------------------

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        characterController.Move(move * speed * Time.deltaTime);

        // Acción de saltar
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Usamos Abs para evitar errores si pusiste la gravedad positiva por error
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    // Esto dibujará una esfera roja/verde en tu ventana de Escena para que veas el sensor
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, sphereRadius);
        }
    }
}