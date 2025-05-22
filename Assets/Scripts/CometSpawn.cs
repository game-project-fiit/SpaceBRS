using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CometSpawner : MonoBehaviour
{
	public GameObject cometPrefab;
	public List<Sprite> cometSpritesLeft;
	public List<Sprite> cometSpritesRight;
	public RectTransform planetRect;
	public float spawnInterval = 6f;
	public float spawnRangeX = 8.0f;
	public float cometSpeed = 7.0f;
	private float screenHeight;
	private Coroutine spawnCoroutine;

	private readonly List<GameObject> activeCometsLeft = new();
	private readonly List<GameObject> activeCometsRight = new();

	private void Start()
	{
		spawnRangeX = Camera.main.orthographicSize * Camera.main.aspect;
		screenHeight = Camera.main.orthographicSize * 2.0f;
	}

	public void StartSpawning()
		=> spawnCoroutine ??= StartCoroutine(SpawnComets());

	public void StopSpawning()
	{
		if (spawnCoroutine == null) return;

		StopCoroutine(spawnCoroutine);
		spawnCoroutine = null;
	}

	private IEnumerator SpawnComets()
	{
		while (true)
		{
			SpawnComet();
			yield return new WaitForSeconds(spawnInterval);
		}
	}

	private void SpawnComet()
	{
		var spawnSide = Random.Range(0, 2);
		Vector3 spawnPosition;

		if (activeCometsLeft.Count >= 2) return;

		spawnPosition = spawnSide == 0
			? new(-spawnRangeX, Random.Range(screenHeight / 3, screenHeight / 2), 0)
			: new(spawnRangeX, Random.Range(screenHeight / 2, screenHeight * 2 / 3), 0);

		SpawnAndTrackComet(spawnPosition, spawnSide);
	}

	private void SpawnAndTrackComet(Vector3 spawnPosition, int spawnSide)
	{
		var comet = Instantiate(cometPrefab, spawnPosition, Quaternion.identity);
		var cometScript = comet.GetComponent<Comet>();
		cometScript.planetRect = planetRect;

		var spriteRenderer = comet.GetComponent<SpriteRenderer>();
		if (spriteRenderer != null)
		{
			spriteRenderer.sprite = spawnSide switch
			{
				0 when cometSpritesLeft.Count > 0
					=> cometSpritesLeft[Random.Range(0, cometSpritesLeft.Count)],
				1 when cometSpritesRight.Count > 0
					=> cometSpritesRight[Random.Range(0, cometSpritesRight.Count)],
				_ => spriteRenderer.sprite
			};
		}

		var rigidBody = comet.GetComponent<Rigidbody2D>();

		if (!rigidBody)
			rigidBody = comet.AddComponent<Rigidbody2D>();

		rigidBody.gravityScale = 0;

		var horizontalSpeed = spawnSide == 0
			? cometSpeed
			: -cometSpeed;

		var verticalSpeed = -cometSpeed;

		rigidBody.linearVelocity = new Vector2(horizontalSpeed, verticalSpeed);

		if (spawnSide == 0)
			activeCometsLeft.Add(comet);
		else
			activeCometsRight.Add(comet);

		Destroy(comet, (screenHeight + 10) / Mathf.Sqrt(cometSpeed * cometSpeed));
	}

	public void ClearAllComets()
	{
		foreach (var comet in activeCometsLeft.Where(comet => comet))
			Destroy(comet);

		foreach (var comet in activeCometsRight.Where(comet => comet))
			Destroy(comet);

		activeCometsLeft.Clear();
		activeCometsRight.Clear();
	}

	private void Update()
	{
		activeCometsLeft.RemoveAll(comet => !comet);
		activeCometsRight.RemoveAll(comet => !comet);
	}
}