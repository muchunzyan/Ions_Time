using UnityEngine;

public class ShootingPointController : MonoBehaviour
{
    public bool canShoot = true;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Bullet") && !col.CompareTag("Player"))
        {
            canShoot = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Bullet") && !other.CompareTag("Player"))
        {
            canShoot = true;
        }
    }
}
