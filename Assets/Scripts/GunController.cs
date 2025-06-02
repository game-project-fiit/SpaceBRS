using UnityEngine;
using UnityEngine.InputSystem;

public class GunShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public RectTransform firePoint;
    public AudioClip shootSound;
    public AudioSource audioSource;
    public GameManager gameManager;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!Keyboard.current.spaceKey.wasPressedThisFrame || !Camera.main || !gameManager.IsGameActive()) return;

        var screenPosition = firePoint.position;
        screenPosition.z = Mathf.Abs(Camera.main.transform.position.z);
        var worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
    
        Instantiate(bulletPrefab, worldPosition, firePoint.rotation);
        PlayShootSound();
    }

    private void PlayShootSound()
    {
        if (shootSound && audioSource)
            audioSource.PlayOneShot(shootSound);
    }
}