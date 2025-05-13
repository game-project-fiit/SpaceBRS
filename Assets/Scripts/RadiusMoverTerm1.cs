using UnityEngine;
using UnityEngine.InputSystem;

public class RadiusMoverTerm1 : MonoBehaviour
{
	public RectTransform planetRect;
	public float angularSpeed = 90f;
	public float rotationSpeed = 10f;
	new Camera camera;
	float cameraZ;

	private void Awake()
	{
		camera = Camera.main;
		cameraZ = Mathf.Abs(camera?.transform.position.z ?? 0);
	}

	private void Update()
	{
		transform.RotateAround(planetRect.transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);

		var input = 0f;
		if (Keyboard.current.aKey.isPressed)
			input += 1f;
		if (Keyboard.current.dKey.isPressed)
			input -= 1f;
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

		if (!outOfBounds)
		{
			((RectTransform)transform).position = newScreen;
			transform.up = (newWorld - worldPlanet).normalized;
		}
	}
}