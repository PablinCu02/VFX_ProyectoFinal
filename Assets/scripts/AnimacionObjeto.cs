using UnityEngine;

public class AnimacionObjeto : MonoBehaviour
{
    [Header("Rotación")]
    public float velocidadRotacion = 50f; // Qué tan rápido da vueltas

    [Header("Flotación")]
    public float amplitudFlote = 0.3f; // Qué tanto sube y baja (distancia)
    public float velocidadFlote = 2f;  // Qué tan rápido hace el viaje senoidal

    private Vector3 posicionInicial;

    void Start()
    {
        // Guardamos su posición original completa para que flote a partir de ahí
        // y no se vaya volando hacia el techo
        posicionInicial = transform.position;
    }

    void Update()
    {
        // 1. Efecto de Giro (sobre su propio eje Y LOCAL)
        // Cambiamos Space.World a Space.Self para que gire sobre sí misma de forma robusta
        transform.Rotate(Vector3.up * velocidadRotacion * Time.deltaTime, Space.Self);

        // 2. Efecto de Flote (Movimiento Senoidal)
        // Calculamos la nueva altura basándonos puramente en posicionInicial.y
        float nuevaY = posicionInicial.y + Mathf.Sin(Time.time * velocidadFlote) * amplitudFlote;

        // Aplicamos la nueva posición Vector3 completa, basada en posicionInicial,
        // para asegurar que no haya desviaciones accidentales en X o Z.
        transform.position = new Vector3(posicionInicial.x, nuevaY, posicionInicial.z);
    }
}