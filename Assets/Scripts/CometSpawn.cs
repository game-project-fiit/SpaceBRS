using System.Collections;
using UnityEngine;

public class CometSpawner : MonoBehaviour
{
    [Header("Comet Settings")]
    public GameObject cometPrefab; 
    public float spawnInterval = 1.0f; 
    public float spawnRangeX = 8.0f; 
    public float cometSpeed = 5.0f; 

    [Header("Spawn Area")]
    public Transform spawnPoint; 

    private float screenHeight;

    private void Start()
    {
        if (spawnPoint == null)
        {
            spawnRangeX = Camera.main.orthographicSize * Camera.main.aspect;
        }

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
        float spawnX = Random.Range(0, spawnRangeX);
        Vector3 spawnPosition = spawnPoint != null
            ? spawnPoint.position
            : new Vector3(spawnX, Camera.main.orthographicSize + 1, 0);
        
        GameObject comet = Instantiate(cometPrefab, spawnPosition, Quaternion.identity);
        
        Rigidbody2D rb = comet.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = comet.AddComponent<Rigidbody2D>();
        }

        rb.gravityScale = 0; 
        
        float diagonalSpeedX = -cometSpeed; 
        float diagonalSpeedY = -cometSpeed; 
        rb.linearVelocity = new Vector2(diagonalSpeedX, diagonalSpeedY);
        
        Destroy(comet, (screenHeight + 10) / Mathf.Sqrt(diagonalSpeedX * diagonalSpeedX + diagonalSpeedY * diagonalSpeedY));
    }
}
