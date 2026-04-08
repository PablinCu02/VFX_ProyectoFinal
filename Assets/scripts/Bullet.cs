using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;

    void OnTriggerEnter(Collider other)
    {
        // Este mensaje debe salir SIEMPRE que toques CUALQUIER cosa
        Debug.Log("LA BALA TOCÓ ALGO: " + other.name + " en la Layer: " + LayerMask.LayerToName(other.gameObject.layer));

        EnemyLogics enemy = other.GetComponent<EnemyLogics>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log("¡CONTACTO CON ENEMIGO CONFIRMADO!");
        }
    }
}