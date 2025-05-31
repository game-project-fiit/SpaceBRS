using UnityEngine;
using UnityEngine.InputSystem;

public class RadiusMover : MonoBehaviour
{
    public RectTransform planetRect;
    public float angularSpeed = 90f;
    private new Camera camera;
    private float cameraZ;
    //public AudioClip stepSound;
    private AudioSource audioSource;

    private void Awake()
    {
        camera = Camera.main;
        cameraZ = Mathf.Abs(camera?.transform.position.z ?? 0);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        var input = 0f;
        var controlScheme = HotkeysPanelController.GetControlScheme();

        if (controlScheme == "Arrows")
        {
            if (Keyboard.current.leftArrowKey.isPressed)
                input += 1f;
            if (Keyboard.current.rightArrowKey.isPressed)
                input -= 1f;
        }
        else
        {
            if (Keyboard.current.aKey.isPressed)
                input += 1f;
            if (Keyboard.current.dKey.isPressed)
                input -= 1f;
        }

        if (input == 0f) return;

        var screenPlanet = planetRect.position;
        screenPlanet.z = cameraZ;
        var worldPlanet = camera.ScreenToWorldPoint(screenPlanet);

        var screenPlayer = ((RectTransform)transform).position;
        screenPlayer.z = cameraZ;
        var worldPlayer = camera.ScreenToWorldPoint(screenPlayer);

        var deltaAngle = angularSpeed * input * Time.deltaTime;
        var direction = worldPlayer - worldPlanet;
        direction = Quaternion.Euler(0, 0, deltaAngle) * direction;
        var newWorld = worldPlanet + direction;

        var newScreen = camera.WorldToScreenPoint(newWorld);
        newScreen.z = 0;

        var outOfBounds =
            newScreen.x < 0f || newScreen.x > Screen.width ||
            newScreen.y < 0f || newScreen.y > Screen.height;

        if (outOfBounds) return;

        ((RectTransform)transform).position = newScreen;
        transform.up = (newWorld - worldPlanet).normalized;
    }
}