using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController characterController;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float speed = 12f;
    public float gravity = -19.81f;
    public float jumpHeight = 1f;
    public float sphereRadius = 0.4f;

    Vector3 velocity;
    bool isGrounded;

    void Update()
    {
        // Protección de referencia
        if (groundCheck == null || characterController == null)
        {
            Debug.LogError("¡Falta asignar el GroundCheck o el CharacterController en el Inspector!");
            return;
        }

        // Detección de suelo
        isGrounded = Physics.CheckSphere(groundCheck.position, sphereRadius, groundMask);

        // Debug para verificar la detección de suelo
        if (Input.GetButtonDown("Jump"))
        {
            if (!isGrounded) Debug.LogWarning("Intentaste saltar pero el script cree que NO estás tocando el suelo.");
            else Debug.Log("Saltando con éxito.");
        }
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
            // Usamos Abs para evitar errores
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    // Metodo para visualizar la esfera de detección de suelo en el editor
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, sphereRadius);
        }
    }
}