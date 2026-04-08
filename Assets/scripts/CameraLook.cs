using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float mouseSensitivity = 180f;

    public Transform playerBody;
    float xRotation = 0; // Corregí el nombre de la variable (tenía una 't' de más)

    void Start()
    {
        // Bloqueamos el cursor para que no se salga de la ventana
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // CAMBIO CLAVE: Usamos GetAxisRaw para eliminar la aceleración/suavizado
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}   