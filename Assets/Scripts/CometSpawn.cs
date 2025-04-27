using System.Collections;
using UnityEngine;

public class CometSpawner : MonoBehaviour
{
    public GameObject cometPrefab;
    public Sprite cometSpriteLeft;
    public Sprite cometSpriteRight;
    public RectTransform planetRect;
    public float spawnInterval = 2f; 
    public float spawnRangeX = 8.0f;
    public float cometSpeed = 7.0f;
    private float screenHeight;
    private Coroutine spawnCoroutine;

    private void Start()
    {
        spawnRangeX = Camera.main.orthographicSize * Camera.main.aspect;
        screenHeight = Camera.main.orthographicSize * 2.0f;
        spawnCoroutine = StartCoroutine(SpawnComets());
    }

    private IEnumerator SpawnComets()
    {
        while (true)
        {
            SpawnComet();
            yield return new WaitForSeconds(spawnInterval);
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

    private void SpawnComet()
    {
        var spawnSide = Random.Range(0, 2);
        var spawnX = spawnSide == 0 ? -spawnRangeX : spawnRangeX;
        var spawnY = Camera.main.orthographicSize + 1; 

        var spawnPosition = new Vector3(spawnX, spawnY, 0); 

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
    
        var diagonalSpeedX = spawnSide == 0 ? cometSpeed : -cometSpeed; 
        var diagonalSpeedY = -cometSpeed; 
        rigidBody.linearVelocity = new Vector2(diagonalSpeedX, diagonalSpeedY);
    
        Destroy(comet, (screenHeight + 10) / 
                       Mathf.Sqrt(diagonalSpeedX * diagonalSpeedX + diagonalSpeedY * diagonalSpeedY));
    }
}
