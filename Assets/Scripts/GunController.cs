using UnityEngine;
using UnityEngine.InputSystem;

public class GunShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // Debug.Log("Координаты перед выстрело --- Player: " + transform.position + " --- FirePoint: " + firePoint.position);
            Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        }
    }
}
