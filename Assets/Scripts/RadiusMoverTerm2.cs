using UnityEngine;
using UnityEngine.InputSystem;


public class RadiusMoverTerm2 : MonoBehaviour
{
	public RectTransform planetRect;
	public float angularSpeed = 90f;
	public RandomPlanetRotator randomPlanetRotator;

	private Camera camera;
	private float cameraZ;
	private float radius;
	private Vector3 worldPlanet;
	private Vector3 direction;

	private void Start()
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

		var controlScheme = HotkeysPanelController.GetControlScheme();

		if (controlScheme == "WASD")
		{
			if (Keyboard.current.aKey.isPressed)
				input -= 1f;
			if (Keyboard.current.dKey.isPressed)
				input += 1f;
		}
		else if (controlScheme == "Arrows")
		{
			if (Keyboard.current.leftArrowKey.isPressed)
				input -= 1f;
			if (Keyboard.current.rightArrowKey.isPressed)
				input += 1f;
		}

		if (input == 0f && randomPlanetRotator.rotationSpeed == 0f)
			return;

		var playerDeltaDegree = -angularSpeed * input * Time.deltaTime;
		var spineDeltaDegree = randomPlanetRotator.rotationSpeed * Time.deltaTime;
		var totalDegree = playerDeltaDegree + spineDeltaDegree;

		var candidateDirection = Quaternion.Euler(0f, 0f, totalDegree) * direction;
		var newWorld = worldPlanet + candidateDirection * radius;
		var newScreen = camera.WorldToScreenPoint(newWorld);
		newScreen.z = 0f;

		var outOfBounds = newScreen.x < 0f 
		                  || newScreen.x > Screen.width
		                  || newScreen.y < 0f 
		                  || newScreen.y > Screen.height;

		if (outOfBounds) return;

		direction = candidateDirection;
		((RectTransform)transform).position = newScreen;
		transform.up = direction;
	}
}