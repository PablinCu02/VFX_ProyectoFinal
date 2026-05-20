using UnityEngine;

public class AnimacionObjeto : MonoBehaviour
{
    [Header("Rotación")]
    public float velocidadRotacion = 50f;

    [Header("Flotación")]
    public float amplitudFlote = 0.3f;
    public float velocidadFlote = 2f;

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * velocidadRotacion * Time.deltaTime, Space.Self);
        float nuevaY = posicionInicial.y + Mathf.Sin(Time.time * velocidadFlote) * amplitudFlote;
        transform.position = new Vector3(posicionInicial.x, nuevaY, posicionInicial.z);
    }
}