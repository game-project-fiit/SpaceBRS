using System.Collections;
using UnityEngine;

public class CometSpawner : MonoBehaviour
{
    public GameObject cometPrefab;
    public RectTransform planetRect;
    public Transform spawnPoint;
    public float spawnInterval = 1.0f; 
    public float spawnRangeX = 8.0f; 
    public float cometSpeed = 5.0f;  
    private float screenHeight;

    private void Start()
    {
        if (spawnPoint == null)
            spawnRangeX = Camera.main.orthographicSize * Camera.main.aspect;

        screenHeight = Camera.main.orthographicSize * 2.0f;
        StartCoroutine(SpawnComets());
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
        var spawnX = Random.Range(0, spawnRangeX);
        var spawnPosition = spawnPoint != null
            ? spawnPoint.position
            : new Vector3(spawnX, Camera.main.orthographicSize + 1, 0);

        var comet = Instantiate(cometPrefab, spawnPosition, Quaternion.identity);
        var cometScript = comet.GetComponent<Comet>();
        cometScript.planetRect = planetRect;

        var rigidBody = comet.GetComponent<Rigidbody2D>();
        if (rigidBody == null)
            rigidBody = comet.AddComponent<Rigidbody2D>();

        rigidBody.gravityScale = 0;

        var diagonalSpeedX = -cometSpeed;
        var diagonalSpeedY = -cometSpeed; 
        rigidBody.linearVelocity = new Vector2(diagonalSpeedX, diagonalSpeedY);
        
        Destroy(comet, (screenHeight + 10) / 
            Mathf.Sqrt(diagonalSpeedX * diagonalSpeedX + diagonalSpeedY * diagonalSpeedY));
    }
}
