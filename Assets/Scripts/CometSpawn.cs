using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometSpawner : MonoBehaviour
{
    public GameObject cometPrefab;
    public Sprite cometSpriteLeft;
    public Sprite cometSpriteRight;
    public RectTransform planetRect;
    public float spawnInterval = 6f; 
    public float spawnRangeX = 8.0f;
    public float cometSpeed = 7.0f;
    private float screenHeight;
    private Coroutine spawnCoroutine;

    private List<GameObject> activeCometsLeft = new();
    private List<GameObject> activeCometsRight = new();

    private void Start()
    {
        spawnRangeX = Camera.main.orthographicSize * Camera.main.aspect;
        screenHeight = Camera.main.orthographicSize * 2.0f;
    }

    public void StartSpawning() 
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnComets());
        }
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine); 
            spawnCoroutine = null;
        }
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

        if (spawnSide == 0) 
        {
            if (activeCometsLeft.Count < 2) 
            {
                spawnPosition = new Vector3(-spawnRangeX, Random.Range(screenHeight / 3, screenHeight / 2), 0); 
                SpawnAndTrackComet(spawnPosition, spawnSide);
            }
        }
        else 
        {
            if (activeCometsRight.Count < 2) 
            {
                spawnPosition = new Vector3(spawnRangeX, Random.Range(screenHeight / 2, screenHeight * 2 / 3), 0); 
                SpawnAndTrackComet(spawnPosition, spawnSide);
            }
        }
    }

    private void SpawnAndTrackComet(Vector3 spawnPosition, int spawnSide)
    {
        var comet = Instantiate(cometPrefab, spawnPosition, Quaternion.identity);
        var cometScript = comet.GetComponent<Comet>();
        cometScript.planetRect = planetRect;

        var spriteRenderer = comet.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = spawnSide == 0 ? cometSpriteLeft : cometSpriteRight;
        }

        var rigidBody = comet.GetComponent<Rigidbody2D>();
        if (rigidBody == null)
            rigidBody = comet.AddComponent<Rigidbody2D>();

        rigidBody.gravityScale = 0;

        var horizontalSpeed = spawnSide == 0 ? cometSpeed : -cometSpeed;
        var verticalSpeed = -cometSpeed;

        rigidBody.linearVelocity = new Vector2(horizontalSpeed, verticalSpeed); // Изменил на velocity

        if (spawnSide == 0)
            activeCometsLeft.Add(comet);
        else
            activeCometsRight.Add(comet);

        Destroy(comet, (screenHeight + 10) / Mathf.Sqrt(cometSpeed * cometSpeed));
    }

    private void Update()
    {
        activeCometsLeft.RemoveAll(comet => comet == null);
        activeCometsRight.RemoveAll(comet => comet == null);
    }
}
