using UnityEngine;
using UnityEngine.InputSystem;

public class GunShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public RectTransform firePoint;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            var screenPosition = firePoint.position;
            screenPosition.z = Mathf.Abs(Camera.main.transform.position.z);
            var worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            Instantiate(bulletPrefab, worldPosition, firePoint.rotation);
        }
    }
}
