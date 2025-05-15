using UnityEngine;
using UnityEngine.InputSystem;


public class RadiusMoverTerm2 : MonoBehaviour
{
    public RectTransform planetRect;
    public float angularSpeed = 90f;
    public RandomPlanetRotator randomPlanetRotator;
    
    Camera camera;
    float cameraZ;
    float radius;
    Vector3 worldPlanet;
    Vector3 direction;

    void Start()
    {
        camera = Camera.main;
        cameraZ = Mathf.Abs(camera.transform.position.z);

        var worldCoordinates = planetRect.position;
        worldCoordinates.z = cameraZ;
        worldPlanet = camera.ScreenToWorldPoint(worldCoordinates);

        var playerTransform = (RectTransform)transform;
        var playerPosition = playerTransform.position;
        playerPosition.z = cameraZ;
        var worldPlayer = camera.ScreenToWorldPoint(playerPosition);

        var offset = worldPlayer - worldPlanet;
        radius = offset.magnitude;
        direction = offset.normalized;
    }
    
    void Update()
    {
        var input = 0f;
        if (Keyboard.current.aKey.isPressed)
            input += 1f;
        if (Keyboard.current.dKey.isPressed)
            input -= 1f;
        if (input == 0f && randomPlanetRotator.rotationSpeed == 0f) return;

        var playerDeltaDegree = angularSpeed * input * Time.deltaTime;
        var spineDeltaDegree = randomPlanetRotator.rotationSpeed * Time.deltaTime;
        var totalDegree = playerDeltaDegree + spineDeltaDegree;

        var candidateDirection = Quaternion.Euler(0f, 0f, totalDegree) * direction;
        var newWorld = worldPlanet + candidateDirection * radius;
        var newScreen = camera.WorldToScreenPoint(newWorld);
        newScreen.z = 0f;

        var outOfBounds = newScreen.x < 0f || newScreen.x > Screen.width
                                           || newScreen.y < 0f || newScreen.y > Screen.height;

        if (!outOfBounds)
        {
            direction = candidateDirection;
            ((RectTransform)transform).position = newScreen;
            transform.up = direction;
        }
    }
}