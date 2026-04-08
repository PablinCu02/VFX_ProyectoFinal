using UnityEngine;

public class ProjectileGun : MonoBehaviour
{
    [Header("Referencias del Proyectil")]
    public GameObject bulletPrefab; // Modelo de bala
    public Transform firePoint;     

    [Header("Ajustes")]
    public float bulletForce = 30f; // Velocidad de la bala
    public float fireRate = 0.5f;   // Tiempo entre disparos
    private float nextTimeToFire = 0f;

    [Header("Munición")]
    public int maxAmmo = 15; // Límite máximo de balas 
    public int currentAmmo;  //Variable para balas actuales

    void Start()
    {
        // Munición completa al iniciar el juego
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + fireRate;
            Shoot();
        }
        else if (Input.GetButtonDown("Fire1") && currentAmmo <= 0)
        {
            Debug.Log("Sin munición! Necesitas recoger más munición");
        }
    }

    void Shoot()
    {
        Debug.Log("¡Disparo realizado! Balas restantes: " + (currentAmmo - 1));
        currentAmmo--; // Se disminuye la munición al disparar

        // 1. Creamos la bala en la posición y rotación del firePoint
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // 2. Obtenemos su Rigidbody para darle fuerza
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // Le damos un impulso hacia adelante
        rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);

        // 3. Destruimos la bala después de 5 segundos para no llenar la memoria
        Destroy(bullet, 5f);
    }
    public void AddAmmo(int amount)
    {
        currentAmmo += amount; // Agregamos 5 balas al recoger munición
        if (currentAmmo > maxAmmo)
        {
            currentAmmo = maxAmmo;
        }

        Debug.Log("Munición recogida! Balas actuales: " + currentAmmo);
    }
}